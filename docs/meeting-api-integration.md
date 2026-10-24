# راهنمای اتصال برنامه دیگر به API ملاقات

این راهنما برای توسعه‌دهنده برنامه ارسال‌کننده است. فراخوانی از سرور برنامه دیگر انجام می‌شود و به ورود کاربر پنل نیاز ندارد.

[روند کامل درخواست ملاقات و ثبت ورود](meeting-requests-workflow.md)

## ۱. تنظیمات و کلید مشترک

بخش زیر در `HerasatUmz/Server/appsettings.json` قرار دارد:

```json
{
  "MeetingApi": {
    "ApiKey": "",
    "RequestIntervalMinutes": 1
  }
}
```

این قطعه فقط بخش مرتبط از تنظیمات را نشان می‌دهد؛ تنظیمات موجود برنامه باید حفظ شوند.

- `ApiKey`: کلید مشترک برنامه مقصد و برنامه ارسال‌کننده. خالی بودن آن باعث رد احراز هویت می‌شود.
- `RequestIntervalMinutes`: حداقل فاصله ثبت درخواست برای یک `HostNationalCode`؛ پیش‌فرض یک دقیقه است و باید عدد صحیح بزرگ‌تر از صفر باشد.
- ملاک محدودیت، `CreatedDate` یعنی زمان ثبت درخواست در سرور است، نه `RequestedDate`.
- محدودیت به نام مراجعه‌کننده یا تاریخ ملاقات وابسته نیست؛ با تغییر این موارد، محدودیت همان کد ملی همچنان برقرار است.
- بررسی زمانی، درخواست‌های قبلی را مستقل از وضعیت آن‌ها در نظر می‌گیرد.

در محیط عملیاتی می‌توان مقادیر را از متغیر محیطی تأمین کرد:

```text
MeetingApi__ApiKey=<SECRET>
MeetingApi__RequestIntervalMinutes=1
```

کلید واقعی در سرور برنامه ارسال‌کننده نیز نگهداری شود؛ آن را در کد مرورگر، اپ موبایل، URL، لاگ یا گیت قرار ندهید.

## ۲. قرارداد API ثبت درخواست

```http
POST https://YOUR_HOST/api/meeting-requests
Content-Type: application/json
X-API-Key: YOUR_SECRET_KEY
```

```json
{
  "hostFullName": "علی رضایی",
  "hostNationalCode": "0012345678",
  "visitorFullName": "محمد احمدی",
  "requestedDate": "2026-10-25"
}
```

قواعد ورودی فعلی:

- نام مراجعه‌کننده و ملاقات‌شونده الزامی و حداکثر ۲۰۰ کاراکتر هستند.
- تاریخ الزامی است و باید قابل تبدیل به `DateOnly` باشد؛ بررسی گذشته یا آینده بودن تاریخ پیاده نشده است.
- برای کد ملی، اعتبارسنجی قالب، طول یا رقم کنترل وجود ندارد. آن را به‌صورت رشته ارسال کنید تا صفر ابتدایی حفظ شود.
- کد ملی به‌صورت رشته غیر `null` ارسال شود؛ فاصله ابتدا و انتهای آن هنگام پردازش حذف می‌شود.

### ثبت موفق — HTTP 201

```json
{
  "success": true,
  "message": "درخواست ملاقات با موفقیت ثبت شد.",
  "id": 125,
  "status": "Pending"
}
```

### کلید نامعتبر، ارسال‌نشده یا تنظیم‌نشده — HTTP 401

```json
{
  "success": false,
  "message": "مشکل در تنظیمات اولیه"
}
```

### محدودیت زمانی — HTTP 429

```json
{
  "success": false,
  "message": "شما نمی‌توانید در فاصله کمتر از 1 دقیقه چند درخواست ثبت کنید."
}
```

عدد پیام مطابق `RequestIntervalMinutes` تغییر می‌کند. پس از سپری شدن فاصله تنظیم‌شده، ثبت مجدد مجاز است.

### سایر خطاها

| HTTP | مفهوم |
|---|---|
| `400` | ورودی نامعتبر یا خطای قواعد ثبت، مانند ورود تکراری یک درخواست |
| `404` | درخواست ملاقات مورد نظر یافت نشد یا فعال نیست؛ در API جزئیات یا ثبت ورود |
| `500` | خطای داخلی سرور یا دیتابیس |

پاسخ خطاها JSON است، اما قالب همه خطاها یکسان نیست. اعتبارسنجی مدل می‌تواند قالب `ValidationProblemDetails` داشته باشد؛ مدیریت خطای عمومی از `code`، `message` و `traceId` استفاده می‌کند. برنامه مصرف‌کننده نباید وجود `success` را در تمام پاسخ‌های خطا فرض کند و باید کد HTTP را بررسی کند.

## ۳. تست در Postman

1. یک درخواست با متد `POST` و آدرس `https://YOUR_HOST/api/meeting-requests` بسازید.
2. در Headers، مقادیر `Content-Type: application/json` و `X-API-Key: YOUR_SECRET_KEY` را وارد کنید.
3. در Body، گزینه `raw` و نوع `JSON` را انتخاب و بدنه نمونه بالا را قرار دهید.
4. درخواست را ارسال کنید؛ بار اول باید `201` دریافت شود.
5. همان کد ملی را قبل از پایان فاصله زمانی دوباره ارسال کنید؛ باید `429` دریافت شود.
6. مقدار هدر کلید را تغییر دهید؛ باید `401` دریافت شود.

به‌جای `YOUR_HOST`، آدرس و پورت واقعی برنامه را وارد کنید. جدول `MeetingRequests` باید با Migration ساخته شده باشد.

## ۴. فراخوانی از سرور یک برنامه دیگر با C#

کلید نمونه زیر باید از تنظیمات امن همان برنامه خوانده شود. از `HttpClient` مدیریت‌شده یا `IHttpClientFactory` استفاده کنید.

```csharp
using System.Net.Http.Json;

public static async Task<string> SendMeetingRequestAsync(
    HttpClient client,
    string apiKey,
    CancellationToken cancellationToken = default)
{
    // BaseAddress باید آدرس واقعی سرور مقصد باشد و به / ختم شود.
    // مثال: https://example.org/
    using var request = new HttpRequestMessage(HttpMethod.Post, "api/meeting-requests");
    request.Headers.Add("X-API-Key", apiKey);
    request.Content = JsonContent.Create(new
    {
        hostFullName = "علی رضایی",
        hostNationalCode = "0012345678",
        visitorFullName = "محمد احمدی",
        requestedDate = "2026-10-25"
    });

    using var response = await client.SendAsync(request, cancellationToken);
    var json = await response.Content.ReadAsStringAsync(cancellationToken);

    if (response.StatusCode == System.Net.HttpStatusCode.Created)
        return json; // شناسه درخواست در id قرار دارد.

    // در برنامه واقعی، کد HTTP و بدنه JSON را به رابط کاربری منتقل کنید.
    // هدر کلید را در گزارش خطا ثبت نکنید.
    throw new InvalidOperationException(
        $"Meeting request failed: HTTP {(int)response.StatusCode}. {json}");
}
```

در `429` ثبت فوری را تکرار نکنید؛ فاصله زمانی تنظیم‌شده را رعایت کنید. در خطای شبکه، ممکن است سرور درخواست را ثبت کرده ولی پاسخ نرسیده باشد؛ API فعلی کلید idempotency یا شناسه یکتای درخواست در برنامه مبدأ ندارد.
