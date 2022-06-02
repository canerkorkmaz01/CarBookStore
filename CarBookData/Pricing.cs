using CarBookData.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBookData
{
    public class Pricing : BaseEntity
    {
        [Display(Name = "Araç Adı")]
        public int CarId { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyWages { get; set; }
        public decimal MonthlyFee { get; set; }
        public virtual Car Cars { get; set; }
    }

    public class PricingEntityTypeConfiguration : IEntityTypeConfiguration<Pricing>
    {
        public void Configure(EntityTypeBuilder<Pricing> builder)
        {
            builder
              .Property(p => p.HourlyRate)
              .IsUnicode(false)
              .IsRequired()
              .HasPrecision(18, 0);
            builder
             .Property(p => p.DailyWages)
             .IsUnicode(false)
             .IsRequired()
             .HasPrecision(18, 0);
            builder
             .Property(p => p.MonthlyFee)
             .IsUnicode(false)
             .IsRequired()
             .HasPrecision(18, 0);
        }
    }
}
