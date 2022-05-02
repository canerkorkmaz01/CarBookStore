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
    public enum Feature:byte
    {
        [Display(Name = "Var")]
        True,
        [Display(Name = "Yok")]
        False
    }
    public class CarFeature : BaseEntity
    {
        public int CarId { get; set; }
        public byte WeatherConditions { get; set; }
        public byte ChildSeat { get; set; }
        public byte SuitCase { get; set; }
        public byte Music { get; set; }
        public byte SafetyBelt { get; set; }
        public byte SleepingBed { get; set; }
        public byte Bluetooth { get; set; }
        public byte OnboardComputer { get; set; }
        public byte AudioInput { get; set; }
        public byte LongTrip { get; set; }
        public byte Toolkit { get; set; }
        public byte RemoteCentralLock { get; set; }
        public byte ClimateControl { get; set; }
        public byte Gps { get; set; }
        public virtual Car Cars { get; set; }
    }

    public class CarFeatureEntityTypeConfiguration : IEntityTypeConfiguration<CarFeature>
    {
        public void Configure(EntityTypeBuilder<CarFeature> builder)
        {
           
        }
    }
}
