using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User.Auth
{
    public class LoginDto
    {
        public string CodeId { get; set; }
        public string Password { get; set; }
    }
}
