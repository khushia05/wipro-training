using BCrypt.Net;

namespace RentAPlace.API.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        bool IsPasswordStrong(string password);
    }

    public class PasswordService : IPasswordService
    {
        private const int WorkFactor = 12; // BCrypt work factor (higher = more secure but slower)

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            // Generate a salt and hash the password using BCrypt
            // BCrypt automatically handles salt generation and storage
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(WorkFactor));
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            if (string.IsNullOrEmpty(passwordHash))
                return false;

            try
            {
                // Verify the password against the stored hash using BCrypt
                // BCrypt automatically extracts the salt from the stored hash
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch (Exception)
            {
                // If verification fails due to malformed hash, return false
                return false;
            }
        }

        public bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Password strength requirements:
            // - At least 8 characters long
            // - Contains at least one uppercase letter
            // - Contains at least one lowercase letter
            // - Contains at least one digit
            // - Contains at least one special character
            if (password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}
