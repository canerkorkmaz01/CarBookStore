using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Enums
{
    public enum Enabled
    {
        [Display(Name = "Aktif")]
        Aktif,
        [Display(Name = "Pasif")]
        Pasif
    }
    public enum Safe
    {
        [Display(Name = "Sedan")]
        Sedan,
        [Display(Name = "Hacpek")]
        Hackpek
    }
    public enum FuelType
    {
        [Display(Name = "Benzin")]
        Benzin,
        [Display(Name = "Dizel")]
        Dizel,
        [Display(Name = "LPG")]
        LPG,
        [Display(Name = "Elektrikli")]
        Elektrikli
    }
    public enum GearType
    {
        [Display(Name = "Manuel")]
        Manuel,
        [Display(Name = "Otomatik")]
        Otomatik,
        [Display(Name = "Triptonik")]
        Triptonik
    }

    public enum Licence : int
    {
        [Display(Name = "A")]
        A,
        [Display(Name = "B1")]
        B1,
        [Display(Name = "B")]
        B,
        [Display(Name = "C1")]
        C1,
        [Display(Name = "CE1")]
        CE1

    }
}
