using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Image
{
    public static class SaveImageAsync
    {
        public static async Task<string> SaveAsync(string base64, string relativeFolder, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Base64 نمی‌تواند خالی باشد.", nameof(base64));

            if (string.IsNullOrWhiteSpace(relativeFolder))
                throw new ArgumentException("مسیر نسبی نمی‌تواند خالی باشد.", nameof(relativeFolder));

            // حذف پیشوند data:image/...;base64, اگر وجود داشت
            var cleanBase64 = base64.Contains(',') ? base64.Split(',')[1] : base64;

            // تولید نام فایل
            var fileName = $"{Guid.NewGuid():N}.jpg";
            var filePath = Path.Combine(relativeFolder, fileName);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            // ایجاد پوشه
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            // ذخیره فایل
            var bytes = System.Convert.FromBase64String(cleanBase64);
            await File.WriteAllBytesAsync(fullPath, bytes, cancellationToken);

            return fileName;
        }
    }
}
