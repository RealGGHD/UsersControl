namespace Task4.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Models;

[Authorize]
public class UsersController : Controller
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }
    
    // GET: /Users
    public async Task<IActionResult> Index()
    {
        var users = await _db.Users
            .OrderByDescending(u => u.LastLoginTime)
            .ToListAsync();

        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid? currentUserId = null;
        if (Guid.TryParse(currentUserIdClaim, out var guid))
        {
            currentUserId = guid;
        }

        var model = new UsersViewModel
        {
            Users = users,
            CurrentUserId = currentUserId
        };

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Block([FromForm] List<Guid> selectedIds)
    {
        if (selectedIds.Count > 0)
        {
            var users = _db.Users.Where(u => selectedIds.Contains(u.Id));
            foreach (var user in users)
            {
                user.Status = UserStatus.Blocked;
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Users blocked.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unblock([FromForm] List<Guid> selectedIds)
    {
        if (selectedIds.Count > 0)
        {
            var users = _db.Users.Where(u => selectedIds.Contains(u.Id));
            foreach (var user in users)
            {
                user.Status = UserStatus.Active;
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Users unblocked.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromForm] List<Guid> selectedIds)
    {
        if (selectedIds.Count > 0)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserGuid = currentUserId != null ? Guid.Parse(currentUserId) : Guid.Empty;

            var usersToDelete = await _db.Users
                .Where(u => selectedIds.Contains(u.Id))
                .ToListAsync();

            bool deletingSelf = usersToDelete.Any(u => u.Id == currentUserGuid);

            _db.Users.RemoveRange(usersToDelete);
            await _db.SaveChangesAsync();

            if (deletingSelf)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            TempData["Message"] = "Users deleted.";
        }
        return RedirectToAction("Index");
    }
}