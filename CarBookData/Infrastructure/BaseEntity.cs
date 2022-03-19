using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBookData.Infrastructure
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        public virtual int UserId { get; set; }

        [Display(Name = "Kayıt T.")]
        public virtual DateTime DateCreated { get; set; }

        [Display(Name = "Aktif")]
        public virtual bool Enabled { get; set; }

        public virtual User User { get; set; }
    }
}
