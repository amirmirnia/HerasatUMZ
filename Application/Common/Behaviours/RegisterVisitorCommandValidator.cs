using Application.Commands.Visitor.RegisterVisitor;
using FluentValidation;

namespace Application.Commands.Visitors.RegisterVisitor
{
    public class RegisterVisitorCommandValidator : AbstractValidator<RegisterVisitorCommand>
    {
        public RegisterVisitorCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام بازدیدکننده الزامی است.")
                .MaximumLength(150).WithMessage("نام نمی‌تواند بیش از ۱۵۰ کاراکتر باشد.")
                .MinimumLength(2).WithMessage("نام باید حداقل ۲ کاراکتر باشد.");

            RuleFor(x => x.NationalCode)
                .NotEmpty().WithMessage("کد ملی الزامی است.")
                .Length(10).WithMessage("کد ملی باید دقیقاً ۱۰ رقم باشد.")
                .Matches(@"^\d{10}$").WithMessage("کد ملی باید فقط شامل اعداد باشد.");

            RuleFor(x => x.HostName)
                .NotEmpty().WithMessage("نام میزبان الزامی است.")
                .MaximumLength(150).WithMessage("نام میزبان نمی‌تواند بیش از ۱۵۰ کاراکتر باشد.")
                .MinimumLength(2).WithMessage("نام میزبان باید حداقل ۲ کاراکتر باشد.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^[\+]?[\d\s\-\(\)]{10,20}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("فرمت شماره تماس معتبر نیست.")
                .MaximumLength(20).WithMessage("شماره تماس نمی‌تواند بیش از ۲۰ کاراکتر باشد.");
        }
    }
}
