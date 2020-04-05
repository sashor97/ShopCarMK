using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentACar.Models
{
    public class PrebaruvajModel
    {
        [Range(0,Int32.MaxValue)]
        [Display(Name = "Цена од")]
        public int cenaOd { get; set; }

        [Range(0, Int32.MaxValue)]
        [Display(Name = "Цена до")]
        public int cenaDo { get; set; }

        [Range(1900,2020)]
        [Display(Name = "Година од")]
        public int godinaOd { get; set; }

        [Range(1900, 2020)]
        [Display(Name = "Година до")]
        public int godinaDo { get; set; }

        [Display(Name = "Менувач")]
        public string Menuvac { get; set; }

        [Display(Name = "Регистрација")]
        public string Registracija { get; set; }

        [Display(Name = "Гориво")]
        public string Gorivo { get; set; }

        [Display(Name = "Локација")]
        public string Lokacija { get; set; }

        [Display(Name = "Сортирање по цена")]
        public string Podredi { get; set; }



        public PrebaruvajModel() {
          
        }
    }
}