using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum RoomStatus
    {
        [Description("در دسترس")]
        Available = 1,
        [Description("در حال تعمیر")]
        UnderMaintenance = 2,
        [Description("رزرو شده")]
        Reserved = 3,
        [Description("خارج از سرویس")]
        OutOfService = 4
    }
}
