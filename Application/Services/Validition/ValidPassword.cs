using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Validition
{
    public static class ValidPassword
    {
        public static bool HasUppercase(string password) =>
      !string.IsNullOrEmpty(password) && password.Any(char.IsUpper);

        public static bool HasLowercase(string password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsLower);

        public static bool HasNumber(string password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsDigit);

        public static bool HasSpecialChar(string password) =>
            !string.IsNullOrEmpty(password) && password.Any(c => "!@#$%^&*()_+-=[]{}|;':\",./<>?".Contains(c));

        // === بررسی کامل پسورد ===
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            return HasUppercase(password)
                && HasLowercase(password)
                && HasNumber(password)
                && HasSpecialChar(password);
        }
    }
}
