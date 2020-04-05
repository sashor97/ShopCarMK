using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentACar.Models
{
    public class Vozilo
    {
        [Key]
        public int VoziloId { get; set; }

        [Required(ErrorMessage = "Моделот е задолжителен")]
        [StringLength(12, ErrorMessage = "Максималната големина на името моделот треба да е 12 карактери")]
        [Display(Name = "Име на Моделот")]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "Сликата е задолжителна")]
        [Display(Name = "Слика од возилото")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Локацијата е задолжителна")]
        [StringLength(20, ErrorMessage = "Максималната големина на името локацијата треба да е 20 карактери")]
        [Display(Name = "Локација на возилото")]
        public string Location { get; set; }

        [Range(0, Int32.MaxValue)]
        [Required(ErrorMessage = "Цената на возилото е задолжителна")]
        [Display(Name = "Цена во евра")]
        public double PriceDay { get; set; }

        [Range(1900, 2020)]
        [Required(ErrorMessage = "Година на производство на возилото е задолжителна")]
        [Display(Name = "Година")]
        public int Godina { get; set; }

        [Required(ErrorMessage = "Типот на гориво на возилото е задолжителна")]
        [Display(Name = "Гориво")]
        public string Gorivo { get; set; }

        [Required(ErrorMessage = "Ова поле е задолжително")]
        [Display(Name = "Менувач")]
        public string Menuvac { get; set; }

        [Required(ErrorMessage = "Регистрацијата возилото е задолжителна")]
        [Display(Name = "Регистрација")]
        public string Registracija { get; set; }

        [Required(ErrorMessage = "Моќноста на возилото е задолжителна")]
        [Display(Name = "Сила на моторот во кw")]
        public int Moknost { get; set; }

        //Godina, Gorivo, Menuvac, Registracija, Moknost

        public int KategorijaId { get; set; }
        public Kategorija Kategorija { get; set; }

        public int ProizvoditelId { get; set; }
        public Proizvoditel Proizvoditel { get; set; }

        public int SopstvenikId { get; set; }
        public Sopstvenik Sopstvenik { get; set; }

        public List<Komentar> Komentari { get; set; }
        

        public Vozilo()
        {
            Komentari = new List<Komentar>();
            
        }
    }
}