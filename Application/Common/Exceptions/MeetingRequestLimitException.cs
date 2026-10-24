namespace Application.Common.Exceptions;

public class MeetingRequestLimitException(int minutes)
    : Exception($"شما نمی‌توانید در فاصله کمتر از {minutes} دقیقه چند درخواست ثبت کنید.");
