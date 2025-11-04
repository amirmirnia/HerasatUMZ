using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User.Auth
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; } = new();
        public string Tokenaccess { get; set; } = string.Empty;
        public string Tokenrefresh { get; set; } = string.Empty;
    }
}
