// Domain/Enums/PlateLetter.cs
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum PlateLetter
    {

        [Description("الف")] alef = 1,
        [Description("ب")] ba = 2,
        [Description("پ")] pa = 3,
        [Description("ت")] ta = 4,
        [Description("ث")] tha = 5,
        [Description("ج")] jim = 6,
        [Description("چ")] cha = 7,
        [Description("ح")] ha = 8,
        [Description("خ")] kha = 9,
        [Description("د")] dal = 10,
        [Description("ذ")] zal = 11,
        [Description("ر")] ra = 12,
        [Description("ز")] za = 13,
        [Description("ژ")] zha = 14,
        [Description("س")] sin = 15,
        [Description("ش")] shin = 16,
        [Description("ص")] sad = 17,
        [Description("ض")] zad = 18,
        [Description("ط")] ta_ = 19,
        [Description("ظ")] za_ = 20,
        [Description("ع")] ayn = 21,
        [Description("غ")] ghayn = 22,
        [Description("ف")] fa = 23,
        [Description("ق")] qaf = 24,
        [Description("ک")] kaf = 25,
        [Description("گ")] gaf = 26,
        [Description("ل")] lam = 27,
        [Description("م")] mim = 28,
        [Description("ن")] nun = 29,
        [Description("و")] vav = 30,
        [Description("ه")] ha_ = 31,
        [Description("ی")] ya = 32
    }
}