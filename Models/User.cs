namespace Task4.Models;
using System.ComponentModel.DataAnnotations;

public enum UserStatus
{
    Active,
    Blocked
}
public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(30)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(30)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(256)]
    public required string PasswordHash { get; set; }

    public DateTime RegistrationTime { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public UserStatus Status { get; set; }
}