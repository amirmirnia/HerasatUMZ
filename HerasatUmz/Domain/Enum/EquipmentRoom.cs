using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EquipmentRoom
    {
        [Description("حمام")]
        Bathroom,
        [Description("سیستم سرمایشی و گرمایشی")]
        AirConditioning,
        [Description("اینترنت")]
        Wifi ,
        [Description("توالت ایرانی")]
        Toalet1,
        [Description("توالت فرهنگی")]
        Toalet2,
        [Description("تلوزیون")]
        TV,
    }
}