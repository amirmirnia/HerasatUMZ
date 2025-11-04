using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum Reservationtatus
    {
        [Description("رزرو شده")]
        Reservation,
        [Description("در انتظار تایید")]
        Pending,
        [Description("لغو شده")]
        Cancelled,
        [Description("تمام شده")]
        Finish
    }
}
