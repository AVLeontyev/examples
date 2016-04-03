using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kindergarten.Models;

namespace Kindergarten.Controllers
{
    public class HomeController : Controller
    {

        private NewsContext _db = new NewsContext();
        ImagesContext _dbi = new ImagesContext();

        [HttpPost] // загрузка файлов
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            ImagesEntry image = new ImagesEntry();
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));

                image.Type = "documents";
                image.Text = fileName;
                image.Image = "Files/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost] // загрузка файлов
        public ActionResult UploadSchedule(HttpPostedFileBase upload)
        {
            ImagesEntry image = new ImagesEntry();
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));

                image.Type = "schedule";
                image.Text = fileName;
                image.Image = "Files/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            //NewsContext _db = new NewsContext();
            IQueryable<ImagesEntry> result = from entry in _dbi.Entries
                                           where entry.Id == Id
                                           select entry;
            _dbi.Entries.Remove(result.First());
            _dbi.SaveChanges();
            //return RedirectToAction("Index");
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Измените этот шаблон, чтобы быстро приступить к работе над приложением ASP.NET MVC.";


            var mostRecentEntries =        // Получает самые последние записи
             (from entry in _db.Entries
              orderby entry.DateAdded descending
              select entry).Take(20);
            ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Страница описания приложения.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Страница контактов.";

            return View();
        }

        public ActionResult Schedule()
        {
            ViewBag.Message = "Расписание занятий.";

            NewsListViewModel model = new NewsListViewModel
            {
                Entries = ViewBag.Entries,
                Images = (from image in _dbi.Entries
                          where image.Type == "schedule"
                          select image)
            };

            return View(model);
        }

        public ActionResult Interesting()
        {
            ViewBag.Message = "На заметку.";

            return View();
        }

        public ActionResult MES()
        {
            ViewBag.Message = "МЧС.";

            return View();
        }

        public ActionResult Medicine()
        {
            ViewBag.Message = "Медицинский блок.";

            return View();
        }

        public ActionResult Food()
        {
            ViewBag.Message = "Здоровое питание.";

            return View();
        }

        public ActionResult Logoped()
        {
            ViewBag.Message = "Страничка логопеда.";

            return View();
        }

        public ActionResult GeneralInformation()
        {
            ViewBag.Message = "Общая информация.";

            return View();
        }

        public ActionResult Staff()
        {
            ViewBag.Message = "Общая информация.";

            return View();
        }

        public ActionResult ToParents()
        {
            ViewBag.Message = "Общая информация.";

            return View();
        }

        public ActionResult Documents()
        {
            ViewBag.Message = "Общая информация.";

            NewsListViewModel model = new NewsListViewModel
            {
                Entries = ViewBag.Entries,
                Images = (from image in _dbi.Entries
                          where image.Type == "documents"
                          select image)
            };

            return View(model);
        }


    }
}
