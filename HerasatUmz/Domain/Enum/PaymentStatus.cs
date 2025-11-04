using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum PaymentStatus
    {
        [Description("موفق")]
        Completed,
        [Description("لغو")]
        Cancelled,
        [Description("نا موفق")]
        Failed
    }
}
