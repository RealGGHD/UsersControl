namespace Task4.Services;
using System;
using System.Text;
using System.Security.Cryptography;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using var sha3 = SHA3_256.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = sha3.ComputeHash(inputBytes);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHashBase64)
    {
        var computedHashBase64 = HashPassword(enteredPassword);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computedHashBase64),
            Convert.FromBase64String(storedHashBase64));
    }
}