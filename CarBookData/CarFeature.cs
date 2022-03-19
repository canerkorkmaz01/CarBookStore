﻿using CarBookData.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBookData
{
    public class CarFeature : BaseEntity
    {
        public bool WeatherConditions { get; set; }
        public bool ChildSeat { get; set; }
        public bool SuitCase { get; set; }
        public bool Music { get; set; }
        public bool SafetyBelt { get; set; }
        public bool SleepingBed { get; set; }
        public bool Bluetooth { get; set; }
        public bool OnboardComputer { get; set; }
        public bool AudioInput { get; set; }
        public bool LongTrip { get; set; }
        public bool Toolkit { get; set; }
        public bool RemoteCentralLock { get; set; }
        public bool ClimateControl { get; set; }
        public bool Gps { get; set; }
        public virtual Car Cars { get; set; }
    }

    public class CarFeatureEntityTypeConfiguration : IEntityTypeConfiguration<CarFeature>
    {
        public void Configure(EntityTypeBuilder<CarFeature> builder)
        {
            builder
             .HasOne(p => p.Cars)
             .WithOne(p => p.CarFeatures)
             .HasForeignKey<Car>(p => p.CarFeaturesId);

        }
    }
}
