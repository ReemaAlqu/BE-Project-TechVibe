using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace src.Utils
{
    public class PasswordComplexity : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;

            // Check if password empty or nor
            if (password == null)
                return false;

            // Check for lenght
            if (password.Length < 8)
                return false;

            // Check for at least one uppercase letter, one lowercase letter, one number, and one special character
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*(),.?""\:{}|<>]");
            return hasLowerCase && hasUpperCase && hasDigit && hasSpecialChar;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.";
        }
    }
}
