using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Validation
{
    using System.Text.RegularExpressions;

    namespace Domain.Common.Validation
    {
        public static class ValidationHelpers
        {
            public static bool IsStrongPassword(string? password)
            {
                if (string.IsNullOrEmpty(password) || password.Length < 12)
                    return false;

                bool hasUpper = Regex.IsMatch(password, "[A-Z]");
                bool hasLower = Regex.IsMatch(password, "[a-z]");
                bool hasDigit = Regex.IsMatch(password, @"\d"); // حداقل ۲ عدد
                bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"); 

                //// جلوگیری از تکرار ساده مثل 'qazwqazw'
                //bool noSimpleRepeat = !Regex.IsMatch(password.ToLower(), @"(.+)\1");

                return hasUpper && hasLower && hasDigit && hasSpecial;
            }


            public static bool IsValidIranianNationalCode(string? code)
            {
                if (string.IsNullOrWhiteSpace(code) || !Regex.IsMatch(code, @"^\d{10}$"))
                    return false;

                var check = int.Parse(code.Substring(9, 1));
                var sum = 0;
                for (int i = 0; i < 9; i++)
                    sum += int.Parse(code[i].ToString()) * (10 - i);

                var remainder = sum % 11;
                return (remainder < 2 && check == remainder) || (remainder >= 2 && check == 11 - remainder);
            }

        }
    }

}
