using System.Security.Cryptography;
using System.Text;

namespace StudentPortfolioBuilder.Data;

public static class PasswordHelper
{
    public static string Hash(string plainText)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));
        return Convert.ToHexString(bytes);
    }

    public static bool Verify(string plainText, string hash) =>
        string.Equals(Hash(plainText), hash, StringComparison.OrdinalIgnoreCase);
}
