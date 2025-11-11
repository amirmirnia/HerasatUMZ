// Application/Commands/Visitor/RegisterVisitor/RegisterVisitorCommandValidator.cs
using FluentValidation;
using Domain.Enums;

namespace Application.Commands.Visitor.RegisterVisitor
{
    public class RegisterVisitorCommandValidator : AbstractValidator<RegisterVisitorCommand>
    {
        public RegisterVisitorCommandValidator()
        {
            // ========== اطلاعات شخصی ==========
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("نام و نام خانوادگی الزامی است.")
                .MaximumLength(100)
                .WithMessage("نام و نام خانوادگی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")
                .MinimumLength(3)
                .WithMessage("نام و نام خانوادگی باید حداقل ۳ کاراکتر باشد.");

            RuleFor(x => x.NationalCode)
                .NotEmpty()
                .WithMessage("کد ملی الزامی است.")
                .Length(10)
                .WithMessage("کد ملی باید دقیقاً ۱۰ رقم باشد.")
                .Matches(@"^\d{10}$")
                .WithMessage("کد ملی فقط شامل اعداد است.");


            RuleFor(x => x.HostName)
                .NotEmpty()
                .WithMessage("نام ملاقات‌شونده الزامی است.")
                .MaximumLength(100)
                .WithMessage("نام ملاقات‌شونده نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20)
                .WithMessage("شماره همراه نمی‌تواند بیشتر از ۲۰ کاراکتر باشد.")
                .Matches(@"^09\d{9}$")
                .WithMessage("شماره همراه باید با ۰۹ شروع شده و ۱۱ رقم باشد (مثل: ۰۹۱۲۳۴۵۶۷۸۹).")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.PhotoBase64)
                .Must(BeValidBase64)
                .WithMessage("عکس شخص نامعتبر است.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhotoBase64));

            // ========== اطلاعات خودرو (فقط وقتی HasVehicle = true) ==========
            RuleFor(x => x.HasVehicle)
                .Equal(true)
                .WithMessage("لطفاً مشخص کنید آیا بازدیدکننده خودرو دارد یا خیر.")
                .When(x => x.HasVehicle); 

            // --- پلاک ---
            RuleFor(x => x.PlatePart1)

                .Length(2)
                .WithMessage("قسمت اول پلاک باید دقیقاً ۲ رقم باشد.")
                .Matches(@"^\d{2}$")
                .WithMessage("قسمت اول پلاک فقط شامل اعداد است.")
                .When(x => x.HasVehicle);

            RuleFor(x => x.PlateLetter)
                .IsInEnum()
                .WithMessage("حرف پلاک نامعتبر است.")
                .When(x => x.HasVehicle);

            RuleFor(x => x.PlatePart3)

                .Length(3)
                .WithMessage("قسمت سوم پلاک باید دقیقاً ۳ رقم باشد.")
                .Matches(@"^\d{3}$")
                .WithMessage("قسمت سوم پلاک فقط شامل اعداد است.")
                .When(x => x.HasVehicle);

            RuleFor(x => x.PlatePart4)
                .MaximumLength(20)
                .WithMessage("کد ایران نمی‌تواند بیشتر از 2 کاراکتر باشد.")
                .When(x => x.HasVehicle);

            // --- سایر اطلاعات خودرو ---
            RuleFor(x => x.Color)
                .MaximumLength(50)
                .WithMessage("رنگ خودرو نمی‌تواند بیشتر از ۵۰ کاراکتر باشد.")
                .When(x => x.HasVehicle && !string.IsNullOrWhiteSpace(x.Color));

            RuleFor(x => x.Brand)
                .MaximumLength(50)
                .WithMessage("برند خودرو نمی‌تواند بیشتر از ۵۰ کاراکتر باشد.")
                .When(x => x.HasVehicle && !string.IsNullOrWhiteSpace(x.Brand));

            RuleFor(x => x.VehicleType)
                .IsInEnum()
                .WithMessage("نوع خودرو نامعتبر است.")
                .When(x => x.HasVehicle && x.VehicleType.HasValue);

            RuleFor(x => x.VehiclePhotoBase64)
                .Must(BeValidBase64)
                .WithMessage("عکس پلاک نامعتبر است.")
                .When(x => x.HasVehicle && !string.IsNullOrWhiteSpace(x.VehiclePhotoBase64));
        }

        // ========== متدهای کمکی ==========

        private bool BeValidBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return true;
            var clean = base64.Contains(',') ? base64.Split(',')[1] : base64;
            return clean.Length % 4 == 0 && clean.All(c => c == '=' || c == '+' || c == '/' || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'));
        }
    }
}