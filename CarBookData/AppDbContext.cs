using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarBookData
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarPicture> CarPictures { get; set; }
        public virtual DbSet<CarFeature> CarFeatures { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Pricing> Pricings { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
    }
}
