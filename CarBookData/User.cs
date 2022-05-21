using Microsoft.AspNetCore.Identity;
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
    public enum Genders
    {
        [Display(Name = "Bay")]
        Male,
        [Display(Name = "Bayan")]
        Female,
        [Display(Name = "Belirtilmemiş")]
        Unspecified
    }

    public class User : IdentityUser<int>
    {
        [Display(Name = "Kaydı Yapan")]
        public string Name { get; set; }

        public bool Enabled { get; set; } = true;

        public Genders Gender { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();
        public virtual ICollection<Feature> CarFeatures { get; set; } = new HashSet<Feature>();
        public virtual ICollection<CarPicture> CarPictures { get; set; } = new HashSet<CarPicture>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public virtual ICollection<Pricing> Pricings { get; set; } = new HashSet<Pricing>();
        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    }

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder
                .HasMany(p => p.Cars)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.CarFeatures)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(p => p.CarPictures)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(p => p.Reservations)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .HasMany(p => p.Pricings)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasMany(p => p.Contacts)
              .WithOne(p => p.User)
              .HasForeignKey(p => p.UserId)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
