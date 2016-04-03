using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kindergarten.Models;

namespace Kindergarten.Controllers
{
    public class PhotoGalleryController : Controller
    {
        //
        // GET: /PhotoGallery/

        ImagesContext _dbi = new ImagesContext();

        public int PageSize = 4;

        public ActionResult Index(int page = 1)
        {
            //var mostRecentEntries =        // Получает самые последние записи
            //    (from entry in _db.Entries
            //     orderby entry.DateAdded descending
            //     select entry).Skip((page - 1) * PageSize).Take(PageSize);
            //ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление

            NewsListViewModel model = new NewsListViewModel
            {
                //Entries = ViewBag.Entries,
                //PagingInfo = new PagingInfo
                //{
                //    CurrentPage = page,
                //    ItemsPerPage = PageSize,
                //    TotalItems = (from entry in _db.Entries select entry).Count()
                //},
                Images = (from image in _dbi.Entries
                          where image.Type == "photogallery"
                          select image)
            };
            return View(model);
        }

        public ViewResult Create()
        {
            NewsEntry model = null;// new NewsEntry();//new NewsEntry();
            return View(model);
        }

        public ActionResult Delete(int Id)
        {
            ImagesEntry image = new ImagesEntry();
            IQueryable<ImagesEntry> result = from entry in _dbi.Entries
                                           where entry.Id == Id
                                           select entry;
            _dbi.Entries.Remove(result.First());
            _dbi.SaveChanges();
            //return RedirectToAction("Index");
            return View();
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult Create(News entry) //Принимает класс в качестве параметра
        {
            ImagesEntry image = new ImagesEntry();

            image.Type = "photogallery";

            if (entry.upload1 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload1.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload1.SaveAs(Server.MapPath("~/Content/Image/PhotoGallery/" + fileName));
                entry.Image1 = image.Image = "Content/Image/PhotoGallery/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload2 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload2.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload2.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image2 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload3 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload3.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload3.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image3 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload4 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload4.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload4.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image4 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload5 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload5.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload5.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image5 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload6 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload6.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload6.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image6 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload7 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload7.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload7.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image7 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload8 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload8.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload8.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image8 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload9 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload9.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload9.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image9 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            if (entry.upload10 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload10.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload10.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image10 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
            }

            _dbi.SaveChanges();

            //return Content("New entry successfully added.");
            return RedirectToAction("Index"); // Перенаправляет обратно к действию Index

            // связывание данных модели (Model binding)
        }
    }
}
