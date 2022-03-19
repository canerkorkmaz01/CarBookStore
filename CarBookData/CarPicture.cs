using CarBookData.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBookData
{
    public class CarPicture : BaseEntity
    {
        public int CarId { get; set; }

        public string Photo { get; set; }

        public virtual Car Cars { get; set; }
    }
    public class CarPictureEntityTypeConfiguration : IEntityTypeConfiguration<CarPicture>
    {
        public void Configure(EntityTypeBuilder<CarPicture> builder)
        {
            builder
               .Property(p => p.Photo)
               .IsUnicode(false);
        }
    }
}
