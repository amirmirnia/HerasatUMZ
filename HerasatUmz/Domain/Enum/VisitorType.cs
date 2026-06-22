using System.ComponentModel;

namespace Domain.Enums
{
    /// <summary>نوع ارباب رجوع (ملاقات‌کننده)</summary>
    public enum VisitorType
    {
        [Description("فرد عادی")]
        Normal = 1,

        [Description("دانشجو")]
        Student = 2,
    }
}
