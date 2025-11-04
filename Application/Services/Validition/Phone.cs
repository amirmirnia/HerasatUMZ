using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services.Validition
{
    public static class Phone
    {
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // شماره‌های ایران مثل 0912... یا +98912...
            string pattern = @"^(?:\+98|0)?9\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}
