namespace Task4.Models;

public enum UserStatus
{
    Active,
    Blocked
}
public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public DateTime RegistrationTime { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public UserStatus Status { get; set; }
}