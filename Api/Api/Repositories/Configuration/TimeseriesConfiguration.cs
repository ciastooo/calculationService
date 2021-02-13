using Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Repositories.Configuration
{
    public class TimeseriesConfiguration : IEntityTypeConfiguration<Timeseries>
    {
        public void Configure(EntityTypeBuilder<Timeseries> builder)
        {
            builder.ToTable("Timeseries");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.Property(t => t.Timestamp).IsRequired();
            builder.Property(t => t.Value).IsRequired().HasColumnType("decimal");
        }
    }
}
