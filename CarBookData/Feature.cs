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
    public class Feature:SortableBaseEntity
    {
        [Display(Name = "Kategori Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Name { get; set; }
      
        public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();

    }

    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {

            builder
                .HasIndex(p => new { p.Name })
                .IsUnique();

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
