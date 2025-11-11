using System.ComponentModel;

namespace Domain.Enums
{
    public enum VehicleType
    {
        [Description("نامشخص")]
        Unknown = 0,

        [Description("سواری معمولی")]
        Sedan = 1,

        [Description("وانت")]
        Pickup = 2,

        [Description("ون")]
        Van = 3,

        [Description("مینی‌بوس")]
        Minibus = 4,

        [Description("اتوبوس")]
        Bus = 5,

        [Description("کامیون")]
        Truck = 6,

        [Description("موتورسیکلت")]
        Motorcycle = 7,

        [Description("تاکسی")]
        Taxi = 8,
    }
}