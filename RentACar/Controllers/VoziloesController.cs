using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentACar.Models;

namespace RentACar.Controllers
{
    public class VoziloesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Voziloes
        public ActionResult Index(int? sortNumber=null)
        {
            
            if (User.IsInRole("Owner"))
            {
                string email = User.Identity.GetUserName();

                var Sopstvenici = db.Sopstvenici.Where(k => k.email == email).First();
                var count = db.Sopstvenici.Where(k => k.email == email).Count();
                if (count == 0)
                {
                    return RedirectToAction("Create", "Korisniks");
                }
                if (sortNumber.HasValue)
                {
                    var vozilaOwner = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.SopstvenikId == Sopstvenici.SopstvenikId);
                    return View(sort(vozilaOwner,sortNumber));
                }
                var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.SopstvenikId == Sopstvenici.SopstvenikId);
                return View(vozila.ToList());
            }
            else
            {
                if (sortNumber.HasValue)
                {
                   return View(sort(db,sortNumber));
                }
                var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik);
                return View(vozila.ToList());
             }
        }

        private object sort(IQueryable<Vozilo> vozilaOwner, int? sortNumber)
        {
            switch (sortNumber)
            {
                case 1:
                    return vozilaOwner
                        .OrderBy(x => x.PriceDay)
                        .ToList();
                case 2:
                    return vozilaOwner
                        .OrderByDescending(x => x.PriceDay)
                        .ToList();
                case 3:
                    return vozilaOwner
                        .OrderBy(x => x.Proizvoditel.Name)
                        .ToList();
                case 4:
                    return vozilaOwner
                        .OrderBy(x => x.ModelName)
                        .ToList();

                case 5:
                    return vozilaOwner
                        .OrderByDescending(x => x.Location)
                        .ToList();
                default:
                    return vozilaOwner
                        .ToList();
            }

        }

        private List<Vozilo> sort(ApplicationDbContext db, int? sortNumber)
        {
            switch (sortNumber)
            {
                case 1:
                    return db.Vozila
                        .Include(v => v.Kategorija)
                        .Include(v => v.Proizvoditel)
                        .Include(v => v.Sopstvenik)
                        .OrderBy(x => x.PriceDay)
                        .ToList();
                case 2:
                    return db.Vozila
                        .Include(v => v.Kategorija)
                        .Include(v => v.Proizvoditel)
                        .Include(v => v.Sopstvenik)
                        .OrderByDescending(x => x.PriceDay)
                        .ToList();
                case 3:
                    return db.Vozila
                        .Include(v => v.Kategorija)
                        .Include(v => v.Proizvoditel)
                        .Include(v => v.Sopstvenik)
                        .OrderBy(x => x.Proizvoditel.Name)
                        .ToList();
                case 4:
                    return db.Vozila
                        .Include(v => v.Kategorija)
                        .Include(v => v.Proizvoditel)
                        .Include(v => v.Sopstvenik)
                        .OrderBy(x => x.ModelName)
                        .ToList();
                   
                case 5:
                    return db.Vozila
                        .Include(v => v.Kategorija)
                        .Include(v => v.Proizvoditel)
                        .Include(v => v.Sopstvenik)
                        .OrderByDescending(x => x.Location)
                        .ToList();
                default:
                    return db.Vozila
                        .Include(v => v.Kategorija)
                        .Include(v => v.Proizvoditel)
                        .Include(v => v.Sopstvenik)
                        .ToList();
            }
            
        }

        // GET: Voziloes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vozilo vozilo = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Include(v => v.Komentari).Include(v=>v.Komentari.Select(k=>k.Korisnik)).First(v => v.VoziloId == id);
            if (vozilo == null)
            {
                return HttpNotFound();
            }
            var ratingList = vozilo.Komentari.Select(k => k.Rating).ToList();
            ViewBag.averageRating = 0;
            if (ratingList.Count > 0)
            {
                ViewBag.averageRating = ratingList.Average();
            }
            return View(vozilo);
        }


        public ActionResult Prebaruvaj()
        {

            List<string> goriva = new List<string>();
            goriva.Add("Одбери");
            goriva.Add("Бензин");
            goriva.Add("Бензин/Плин");
            goriva.Add("Дизел");
            goriva.Add("Хибрид");
            goriva.Add("Електричен");
            ViewBag.Gorivo = goriva;

            List<string> podredi = new List<string>();
            podredi.Add("Растечки");
            podredi.Add("Опаѓачки");
            ViewBag.Podredi = podredi;

            List<string> menuvac = new List<string>();
            menuvac.Add("Одбери");
            menuvac.Add("Автоматски");
            menuvac.Add("Рачен");
            ViewBag.Menuvac = menuvac;

            List<string> registracija = new List<string>();
            registracija.Add("Одбери");
            registracija.Add("Македонска");
            registracija.Add("Странска");
            ViewBag.Registracija = registracija;

            List<string> lokacija = new List<string>();
            lokacija.Add("Одбери");
            lokacija.Add("Скопје");
            lokacija.Add("Берово");
            lokacija.Add("Битола");
            lokacija.Add("Валандово");
            lokacija.Add("Виница");
            lokacija.Add("Велес");
            lokacija.Add("Гевгелија");
            lokacija.Add("Гостивар");
            lokacija.Add("Дебар");
            lokacija.Add("Делчево");
            lokacija.Add("Демир Капија");
            lokacija.Add("Дојран");
            lokacija.Add("Зрновци");
            lokacija.Add("Кавадарци");
            lokacija.Add("Кичево");
            lokacija.Add("Кочани");
            lokacija.Add("Кратово");
            lokacija.Add("Крива Паланка");
            lokacija.Add("Крушево");
            lokacija.Add("Куманово");
            lokacija.Add("Македонска Каменица");
            lokacija.Add("Македонски Брод");
            lokacija.Add("Неготино");
            lokacija.Add("Охрид");
            lokacija.Add("Пехчево");
            lokacija.Add("Прилеп");
            lokacija.Add("Пробиштип");
            lokacija.Add("Радовиш");
            lokacija.Add("Свети Никола");
            lokacija.Add("Струга");
            lokacija.Add("Струмица");
            lokacija.Add("Тетово");
            lokacija.Add("Чашка");
            lokacija.Add("Чешиново-Облешево");
            lokacija.Add("Штип");
            ViewBag.Lokacija = lokacija;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrebaruvajRezultati([Bind(Include = "cenaOd, cenaDo, godinaOd, godinaDo, Gorivo, Menuvac, Registracija, Podredi, Lokacija ")] PrebaruvajModel model)
        {
            if (model.Podredi == "Растечки")
            {
                
                if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd &&  v.Menuvac == model.Menuvac ).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Registracija == model.Registracija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac ).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo  && v.Registracija == model.Registracija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd  && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }





                if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Menuvac == model.Menuvac && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                else //if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderBy(x => x.PriceDay);
                    return View(vozila.ToList());
                }




            }
            else
            {
                if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери"  && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Menuvac == model.Menuvac).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Registracija == model.Registracija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Registracija == model.Registracija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija == "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }







                if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Menuvac == model.Menuvac && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo == "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac != "Одбери" && model.Registracija == "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Menuvac == model.Menuvac && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                if (model.Gorivo != "Одбери" && model.Menuvac == "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Gorivo == model.Gorivo && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }

                 else //  if (model.Gorivo == "Одбери" && model.Menuvac != "Одбери" && model.Registracija != "Одбери" && model.Lokacija != "Одбери")
                {
                    var vozila = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.Godina >= model.godinaOd && v.Godina <= model.godinaDo && v.PriceDay <= model.cenaDo && v.PriceDay >= model.cenaOd && v.Menuvac == model.Menuvac && v.Registracija == model.Registracija && v.Location == model.Lokacija).OrderByDescending(x => x.PriceDay);
                    return View(vozila.ToList());
                }






                
            }
            
           
        }

        [Authorize(Roles = "Administrator, Owner")]
        // GET: Voziloes/Create
        public ActionResult Create()
        {
            ViewBag.KategorijaId = new SelectList(db.Kategorii, "KategorijaId", "Name");
            ViewBag.ProizvoditelId = new SelectList(db.Proizvoditeli, "ProizvoditelId", "Name");
            List<string> goriva = new List<string>();
            goriva.Add("Бензин");
            goriva.Add("Бензин/Плин");
            goriva.Add("Дизел");
            goriva.Add("Хибрид");
            goriva.Add("Електричен");
            ViewBag.Gorivo = goriva;

            List<string> menuvac = new List<string>();
            menuvac.Add("Автоматски");
            menuvac.Add("Рачен");
            ViewBag.Menuvac = menuvac;

            List<string> registracija = new List<string>();
            registracija.Add("Македонска");
            registracija.Add("Странска");
            ViewBag.Registracija = registracija;


            if (User.IsInRole("Owner"))
            {
                string email = User.Identity.GetUserName();
                var Sopstvenik = db.Sopstvenici.Where(k => k.email == email).FirstOrDefault();
                ViewBag.SopstvenikId = Sopstvenik.SopstvenikId;
            }
            else
            {
                ViewBag.SopstvenikId = new SelectList(db.Sopstvenici, "SopstvenikId", "Name");
            }
            return View();
        }

        // POST: Voziloes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        //Godina, Gorivo, Menuvac, Registracija, Moknost

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VoziloId,ModelName,ImageUrl,Location,PriceDay,Godina, Gorivo, Menuvac, Registracija, Moknost, KategorijaId,ProizvoditelId,SopstvenikId")] Vozilo vozilo)
        {
            if (ModelState.IsValid)
            {
                db.Vozila.Add(vozilo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategorijaId = new SelectList(db.Kategorii, "KategorijaId", "Name", vozilo.KategorijaId);
            ViewBag.ProizvoditelId = new SelectList(db.Proizvoditeli, "ProizvoditelId", "Name", vozilo.ProizvoditelId);

            List<string> goriva = new List<string>();
            goriva.Add("Бензин");
            goriva.Add("Бензин/Плин");
            goriva.Add("Дизел");
            goriva.Add("Хибрид");
            goriva.Add("Електричен");
            ViewBag.Gorivo = goriva;

            List<string> menuvac = new List<string>();
            menuvac.Add("Автоматски");
            menuvac.Add("Рачен");
            ViewBag.Menuvac = menuvac;

            List<string> registracija = new List<string>();
            registracija.Add("Македонска");
            registracija.Add("Странска");
            ViewBag.Registracija = registracija;

            if (User.IsInRole("Owner"))
            {
                string email = User.Identity.GetUserName();
                var Sopstvenik = db.Sopstvenici.Where(k => k.email == email).FirstOrDefault();
                ViewBag.SopstvenikId = Sopstvenik.SopstvenikId;
            }
            else
            {
                ViewBag.SopstvenikId = new SelectList(db.Sopstvenici, "SopstvenikId", "Name", vozilo.SopstvenikId);
            }
            return View(vozilo);
        }

        [Authorize(Roles = "Administrator, Owner")]
        // GET: Voziloes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vozilo vozilo = db.Vozila.Find(id);
            if (vozilo == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategorijaId = new SelectList(db.Kategorii, "KategorijaId", "Name", vozilo.KategorijaId);
            ViewBag.ProizvoditelId = new SelectList(db.Proizvoditeli, "ProizvoditelId", "Name", vozilo.ProizvoditelId);
            ViewBag.SopstvenikId = new SelectList(db.Sopstvenici, "SopstvenikId", "Name", vozilo.SopstvenikId);
            List<string> goriva = new List<string>();
            goriva.Add("Бензин");
            goriva.Add("Бензин/Плин");
            goriva.Add("Дизел");
            goriva.Add("Хибрид");
            goriva.Add("Електричен");
            ViewBag.Gorivo = goriva;

            List<string> menuvac = new List<string>();
            menuvac.Add("Автоматски");
            menuvac.Add("Рачен");
            ViewBag.Menuvac = menuvac;

            List<string> registracija = new List<string>();
            registracija.Add("Македонска");
            registracija.Add("Странска");
            ViewBag.Registracija = registracija;

            return View(vozilo);
        }

        // POST: Voziloes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VoziloId,ModelName,ImageUrl,Location,PriceDay,Godina, Gorivo, Menuvac, Registracija, Moknost, KategorijaId,ProizvoditelId,SopstvenikId")] Vozilo vozilo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vozilo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategorijaId = new SelectList(db.Kategorii, "KategorijaId", "Name", vozilo.KategorijaId);
            ViewBag.ProizvoditelId = new SelectList(db.Proizvoditeli, "ProizvoditelId", "Name", vozilo.ProizvoditelId);
            ViewBag.SopstvenikId = new SelectList(db.Sopstvenici, "SopstvenikId", "Name", vozilo.SopstvenikId);

            List<string> goriva = new List<string>();
            goriva.Add("Бензин");
            goriva.Add("Бензин/Плин");
            goriva.Add("Дизел");
            goriva.Add("Хибрид");
            goriva.Add("Електричен");
            ViewBag.Gorivo = goriva;

            List<string> menuvac = new List<string>();
            menuvac.Add("Автоматски");
            menuvac.Add("Рачен");
            ViewBag.Menuvac = menuvac;

            List<string> registracija = new List<string>();
            registracija.Add("Македонска");
            registracija.Add("Странска");
            ViewBag.Registracija = registracija;

            return View(vozilo);
        }

        [Authorize(Roles = "Administrator, Owner")]
        // GET: Voziloes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vozilo vozilo = db.Vozila.Include(v => v.Kategorija).Include(v => v.Proizvoditel).Include(v => v.Sopstvenik).Where(v => v.VoziloId == id).FirstOrDefault(); 
            if (vozilo == null)
            {
                return HttpNotFound();
            }
            return View(vozilo);
        }

        // POST: Voziloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vozilo vozilo = db.Vozila.Find(id);
            db.Vozila.Remove(vozilo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
