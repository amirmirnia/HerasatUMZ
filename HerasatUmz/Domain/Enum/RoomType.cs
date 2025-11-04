using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum RoomType
    {
        [Description("کنفرانس")]
        ConferenceRoom=0,
        [Description("ملاقات")]
        MeetingRoom=1,
        [Description("مسافر")]
        TrainingRoom=2,
        [Description("کلاس درس")]
        Classroom=3,
    }
}
