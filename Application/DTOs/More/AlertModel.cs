using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.More
{
    public class AlertModel
    {
        public int Id { get; set; }
        public string Message { get; set; } = "";
        public string Title { get; set; } = "";
        public AlertType Type { get; set; }
    }
}
