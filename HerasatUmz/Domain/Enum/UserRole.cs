using System.ComponentModel;

namespace Domain.Enum;

public enum UserRole
{
    [Description("ادمین")]
    Admin = 1,
    [Description("مدیر")]
    Manager = 2,
    [Description("کاربر")]
    User = 3,



}