namespace Task4.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Models;
using Task4.Services;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }
    
    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
    
    // POST: /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Please fill in all the fields!";
            return View();
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email.ToLower(),
            PasswordHash = PasswordHasher.HashPassword(password),
            Status = UserStatus.Active,
            RegistrationTime = DateTime.UtcNow,
            LastLoginTime = DateTime.UtcNow,
        };

        try
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ViewBag.Error = "User already exists with same email!";
            return View();
        }
        
        return RedirectToAction("Login");
    }
    
    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    
    // POST: /Account/Login
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

        if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
        {
            ViewBag.Error = "Wrong login or password!";
            return View();
        }

        if (user.Status == UserStatus.Blocked)
        {
            ViewBag.Error = "User is blocked!";
            return View();
        }

        user.LastLoginTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Users");
    }
    
    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}