
using System.Globalization;
using System.Reflection;


namespace Application.Services.Convert.Data
{
    public static class DateConverter
    {
        private static CultureInfo _Culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ي", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "يکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd, MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }
        public static DateTime ConvertPersianStringToMiladi(string persianDateTimeStr)
        {
            if (string.IsNullOrWhiteSpace(persianDateTimeStr))
                throw new ArgumentException("رشته ورودی خالی یا نامعتبر است.");

            try
            {
                // جداسازی بخش تاریخ و زمان (با فرض فرمت "M/d/yyyy hh:mm:ss tt")
                string[] mainParts = persianDateTimeStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string dateStr = mainParts[0];
                string timeStr = mainParts.Length > 1 ? mainParts[1] : "12:00:00 AM"; // پیش‌فرض نیمه‌شب

                // تجزیه زمان (پشتیبانی از AM/PM)
                DateTime timePart = DateTime.Parse(timeStr);
                int hour = timePart.Hour;
                int minute = timePart.Minute;
                int second = timePart.Second;

                // تجزیه تاریخ شمسی (فرمت M/d/yyyy)
                string[] dateParts = dateStr.Split('/');
                if (dateParts.Length != 3)
                    throw new ArgumentException("فرمت تاریخ نامعتبر است. از M/d/yyyy استفاده کنید.");

                int pMonth = int.Parse(dateParts[0]);
                int pDay = int.Parse(dateParts[1]);
                int pYear = int.Parse(dateParts[2]);

                // اعتبارسنجی ساده
                if (pMonth < 1 || pMonth > 12 || pDay < 1 || pDay > 31 || pYear < 1)
                    throw new ArgumentException("مقادیر ماه، روز یا سال نامعتبر است.");

                // تبدیل با تقویم پارسی
                PersianCalendar persianCalendar = new PersianCalendar();
                DateTime gregorianDate = persianCalendar.ToDateTime(pYear, pMonth, pDay, hour, minute, second, 0);

                return gregorianDate;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"خطا در تبدیل تاریخ: {ex.Message}", ex);
            }
        }


        public static string ToPeString(this DateTime date, string format = "yyyy/MM/dd")
        {
            return date.ToString(format, GetPersianCulture());
        }

        public static DateTime ToPersianDateTime(DateTime date)
        {
            // ایجاد نمونه از تقویم شمسی
            PersianCalendar persianCalendar = new PersianCalendar();

            // استخراج سال، ماه و روز شمسی
            int year = persianCalendar.GetYear(date);
            int month = persianCalendar.GetMonth(date);
            int day = persianCalendar.GetDayOfMonth(date);

            // ایجاد DateTime معادل با استفاده از سال، ماه و روز شمسی
            return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
        public static int GetYearPersianInDateTime(this DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            int year = persianCalendar.GetYear(date);
            return year;
        }
    }
}
