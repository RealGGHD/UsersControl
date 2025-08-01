namespace Task4.Models;

public class UsersViewModel
{
    public List<User> Users { get; set; } = new List<User>();
    public Guid? CurrentUserId { get; set; }
}