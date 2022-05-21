using CarBookData.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarBookStoreWeb.Enums;

namespace CarBookData
{
     public class Car : BaseEntity
    {
        [Display(Name = "Araç Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string CarName { get; set; }

        [Display(Name = "Yılı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public int Year { get; set; }

        [Display(Name = "Kasa")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public Safe Safe { get; set; }

        [Display(Name = "Yakıt Tipi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public FuelType FuelType { get; set; }

        [Display(Name = "Vites Tipi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public GearType GearType { get; set; }

        [Display(Name = "Kilometresi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public decimal Kilometer { get; set; }

        [Display(Name = "Koltuk Sayısı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public int Armchair { get; set; }

        [Display(Name = "Valiz Sayısı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public int SuitCase { get; set; }

        [Display(Name = "Ehliyeti")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public Licence Licence { get; set; }

        [Display(Name = "Araç Plakası")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Plate { get; set; }

        public string Photo { get; set; }
        [NotMapped]
        public int[] CarFeature { get; set; }


        [NotMapped]
        [Display(Name = "Foto")]
        public IFormFile PhotoFile { get; set; }

        [NotMapped]
        [Display(Name = "Foto Galeri")]
        public IFormFile[] PhotoFiles { get; set; }

        public virtual ICollection<CarPicture> CarPictures { get; set; } = new HashSet<CarPicture>();
        public virtual ICollection<Feature> Features { get; set; } = new HashSet<Feature>();
        public virtual Reservation Reservations { get; set; }
        public virtual Pricing Pricings { get; set; }
    }

    public class CarEntityTypeConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {

            builder
                .HasIndex(p => new { p.CarName })
                .IsUnique();
            builder
                .HasIndex(p => new { p.Year });
                //.IsUnique();
            builder
                .HasIndex(p => new { p.Safe });
                 //.IsUnique();
            builder
                .HasIndex(p => new { p.FuelType });
                 //.IsUnique();
            builder
                .HasIndex(p => new { p.GearType });
                 //.IsUnique();
            builder
                .HasIndex(p => new { p.Kilometer });
                 //.IsUnique();
            builder
                .HasIndex(p => new { p.Armchair });
                 //.IsUnique();
            builder
                .HasIndex(p => new { p.SuitCase });
                 //.IsUnique();
            builder
                .HasIndex(p => new { p.Licence });
                 //.IsUnique();
            builder
              .HasIndex(p => new { p.Plate })
               .IsUnique();

            builder
                .Property(p => p.CarName)
                .IsUnicode(false)
                .HasMaxLength(200)
                .IsRequired();
            builder
               .Property(p => p.Year)
               .IsUnicode(false)
               .HasMaxLength(4)
               .IsRequired();
            builder
               .Property(p => p.Safe)
               .IsUnicode(false)
                .HasMaxLength(200)
               .IsRequired();
            builder
               .Property(p => p.FuelType)
                .IsUnicode(false)
               .HasMaxLength(200)
               .IsRequired();
            builder
               .Property(p => p.GearType)
               .IsUnicode(false)
               .HasMaxLength(200)
               .IsRequired();

            builder
               .Property(p => p.Armchair)
               .IsUnicode(false)
               .HasMaxLength(200)
               .IsRequired();
            builder
               .Property(p => p.SuitCase)
              .IsUnicode(false)
               .HasMaxLength(200)
               .IsRequired();
            builder
               .Property(p => p.Licence)
                .IsUnicode(false)
               .HasMaxLength(200)
               .IsRequired();

            builder
                .Property(p => p.Plate)
                .IsUnicode(false)
                .HasMaxLength(200)
                .IsRequired();

            builder
              .Property(p => p.Kilometer)
              .IsUnicode(false)
              .IsRequired()
              .HasPrecision(18,0);

            builder
              .HasMany(p => p.CarPictures)
              .WithOne(p => p.Car)
              .HasForeignKey(p => p.CarId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
