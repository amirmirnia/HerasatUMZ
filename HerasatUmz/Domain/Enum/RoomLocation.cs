using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum RoomLocation
    {

        [Description("مشهد")]
        Mashad = 0,
        [Description("نور")]
        Nor =1,
        [Description("تهران")]
        Tehran =2,
            [Description("همه")]
        All = 3
    }

}
