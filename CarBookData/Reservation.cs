using CarBookData.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarBookStoreWeb.Enums;

namespace CarBookData
{
    public class Reservation : BaseEntity
    {
        [Display(Name = "Araç Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public int CarId { get; set; }

        [Display(Name = "Adı Soyadı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string NameSurname { get; set; }

        [Display(Name = "Telefonu")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Telephone { get; set; }

        [Display(Name = "Email Adresi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Email { get; set; }

        [Display(Name = "Kiralama Tarihi")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RentalDate { get; set; }

        [Display(Name = "Alış Tarihi")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate{ get; set; }

        [Display(Name = "Teslim Tarihi")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }


        [Display(Name = "Yakıt Tipi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public FuelType FuelType { get; set; }

        [Display(Name = "Vites Tipi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public GearType GearType { get; set; }
        public virtual Car Cars { get; set; }
    }

    public class ReservationEntityTypeConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {

            builder
                .Property(p => p.NameSurname)
                .IsUnicode(false)
                .HasMaxLength(100)
                .IsRequired();
            builder
               .Property(p => p.Telephone)
               .IsUnicode(false)
               .HasMaxLength(14)
               .IsRequired();
            builder
               .Property(p => p.Email)
               .IsUnicode(false)
               .HasMaxLength(100)
               .IsRequired();
            builder
               .Property(p => p.FuelType)
               .IsUnicode(false)
               .HasMaxLength(50)
               .IsRequired();
            builder
                .Property(p => p.GearType)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
