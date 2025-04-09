namespace DocumentManagement.Utility
{

    /// <summary>
    /// provides helper methods for password hashing
    /// </summary>
    public interface IPasswordHasher
    {
        string GetPasswordHash(string password);
        bool VerifyPasswordHash(string password, string passwordHash);

    }

    /// <summary>
    /// provides helper methods for password hashing
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        public string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPasswordHash(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
