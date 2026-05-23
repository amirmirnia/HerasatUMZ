namespace Application.Common.Errors;

/// <summary>
/// Standard error shape returned by every failing API endpoint.
/// Clients should parse this — never display raw response text.
/// </summary>
public class ApiError
{
    /// <summary>Machine-readable code (e.g. "validation_failed", "not_found").</summary>
    public string Code { get; set; } = "internal_error";

    /// <summary>Human-readable Persian message safe to show to end users.</summary>
    public string Message { get; set; } = "خطای پیش‌بینی نشده‌ای رخ داد. لطفاً دوباره تلاش کنید.";

    /// <summary>Per-field validation errors (only set for validation failures).</summary>
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>Correlates server logs with client reports.</summary>
    public string? TraceId { get; set; }
}
