using System.Security.Cryptography;

namespace AuthService;

public static class SessionGenerator
{
    public static Session CreateSession(Guid userId)
    {
        return new Session {SessionKey = GenerateSessionKey(), UserId = userId};
    }

    private static string GenerateSessionKey(int length = 32)
    {
        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            var randomNumber = new byte[length];
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
