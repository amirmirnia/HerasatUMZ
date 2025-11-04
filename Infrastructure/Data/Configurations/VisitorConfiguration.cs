using Domain.Entities.Visitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VisitorConfiguration : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.FullName)
               .IsRequired()
               .HasMaxLength(100)
               .HasComment("نام و نام خانوادگی ملاقات‌کننده");

        builder.Property(v => v.NationalCode)
               .IsRequired()
               .HasMaxLength(10)
               .HasComment("کد ملی ملاقات‌کننده");

        builder.Property(v => v.HostName)
               .IsRequired()
               .HasMaxLength(100)
               .HasComment("نام ملاقات‌شونده در دانشگاه");

        builder.Property(v => v.RegisterDateTime)
               .IsRequired()
               .HasComment("تاریخ و زمان ورود به دانشگاه");

        builder.Property(v => v.ExitDateTime)
               .HasComment("تاریخ و زمان خروج از دانشگاه");

        builder.Property(v => v.GuidCode)
               .IsRequired()
               .HasMaxLength(10)
               .HasComment("کد یکتا برای پیگیری ملاقات");

        builder.Property(v => v.PhoneNumber)
               .HasMaxLength(15)
               .HasComment("شماره همراه ملاقات‌کننده");

        builder.Property(v => v.PhotoPath)
               .HasMaxLength(300)
               .HasComment("مسیر ذخیره تصویر ملاقات‌کننده");

        builder.Property(v => v.IsInside)
               .IsRequired()
               .HasDefaultValue(true)
               .HasComment("وضعیت حضور ملاقات‌کننده در داخل دانشگاه (true = داخل)");

        builder.Property(v => v.CreatedBy)
               .HasMaxLength(100)
               .HasComment("کاربری که این رکورد را ثبت کرده است");

        builder.Property(v => v.UpdatedBy)
               .HasMaxLength(100)
               .HasComment("کاربری که آخرین تغییر را انجام داده است");

        // Indexes
        builder.HasIndex(v => v.NationalCode)
               .HasDatabaseName("IX_Visitor_NationalCode");

        builder.HasIndex(v => v.GuidCode)
               .IsUnique()
               .HasDatabaseName("IX_Visitor_GuidCode");

        builder.HasIndex(v => v.RegisterDateTime)
               .HasDatabaseName("IX_Visitor_RegisterDateTime");

        builder.HasIndex(v => v.IsInside)
               .HasDatabaseName("IX_Visitor_IsInside");
    }
}
