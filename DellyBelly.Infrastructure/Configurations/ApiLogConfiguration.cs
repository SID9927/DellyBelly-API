using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DellyBelly.Infrastructure.Configurations
{
    public class ApiLogConfiguration : IEntityTypeConfiguration<ApiLog>
    {
        public void Configure(EntityTypeBuilder<ApiLog> builder)
        {
            builder.HasKey(x => x.Id);

            // Force 3 decimal places (milliseconds)
            builder.Property(x => x.RequestTime)
                   .HasColumnType("datetime2(3)"); // This is SQL Server specific syntax for 3 digits of precision

            builder.Property(x => x.ResponseTime)
                   .HasColumnType("datetime2(3)");

            builder.Property(x => x.Path).HasMaxLength(256);
            builder.Property(x => x.Method).HasMaxLength(10);
            builder.Property(x => x.IpAddress).HasMaxLength(45);
            builder.Property(x => x.QueryString).HasMaxLength(4000);
            builder.Property(x => x.Duration).HasMaxLength(20);

            // IMPORTANT: cap at 4000 so SQL Server uses nvarchar(4000) instead of
            // nvarchar(MAX). nvarchar(MAX) columns are stored as LOB pages and can
            // cause significant INSERT slowdowns. The middleware truncates bodies to
            // the same limit before writing, so no data is silently dropped.
            builder.Property(x => x.RequestBody).HasMaxLength(4000);
            builder.Property(x => x.ResponseBody).HasMaxLength(4000);
        }
    }
}
