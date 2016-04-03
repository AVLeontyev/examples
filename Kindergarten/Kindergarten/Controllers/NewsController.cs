using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kindergarten.Models;
using System.IO;

namespace Kindergarten.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/

        //private 
            NewsContext _db = new NewsContext();
            ImagesContext _dbi = new ImagesContext();

        public int PageSize = 4;
        public ViewResult Index(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
                (from entry in _db.Entries
                 orderby entry.DateAdded descending
                 select entry).Skip((page - 1) * PageSize).Take(PageSize);
            ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление
    
            NewsListViewModel model = new NewsListViewModel
            {
                Entries = ViewBag.Entries,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = (from entry in _db.Entries select entry).Count()
                },
                Images = (from image in _dbi.Entries
                          where image.Type == "news"
                          select image)
            };
            
        
            //delegate NewsEntry del(int Id);
            //del myDelegate = entry => entry.Id;
            ////int j = myDelegate(5); //j = 25


            //return View(mostRecentEntries.ToList()
            //  .OrderBy(p => p.Id)
            //  .Skip((page - 1) * PageSize)
            //  .Take(PageSize));

            return View(model);
        }

        //public ActionResult Create()
        //{
        //    return View();
        //}

        public ViewResult Create()
        {
            NewsEntry model = null;// new NewsEntry();//new NewsEntry();
            return View(model);
        }

        public ActionResult SuccessCreate()
        {
            return View();
        }

        public ActionResult Change(int Id)
        {
            var mostRecentEntries =        // Получает самые последние записи
                from entry in _db.Entries
                 where entry.Id == Id
                 select entry;
            ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление

            NewsListViewModel model = new NewsListViewModel
            {
                Entries = ViewBag.Entries,
                Images = (from image in _dbi.Entries
                          where image.Type == "news"
                          select image)
            };

            IQueryable<NewsEntry> result = from entry in _db.Entries
                                           where entry.Id == Id
                                           select entry;
            _db.Entries.Remove(result.First());
            _db.SaveChanges();

            return View(model);
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult Change(News entry) //Принимает класс в качестве параметра
        {
            ImagesEntry image = new ImagesEntry();
            NewsEntry entry2 = new NewsEntry();

            entry2.Text = entry.Text;
            entry2.Title = entry.Title;

            entry2.DateAdded = DateTime.Now;
            _db.Entries.Add(entry2); // Сохраняет запись гостевой книги
            _db.SaveChanges();

            image.Type = "news";
            image.Feedback = entry2.Id;

            if (entry.upload1 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload1.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload1.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image1 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
            }

            _dbi.SaveChanges();

            //return Content("New entry successfully added.");
            return RedirectToAction("SuccessCreate"); // Перенаправляет обратно к действию Index

            // связывание данных модели (Model binding)
        }


        public ActionResult Delete(int Id)
        {
            NewsContext _db = new NewsContext();
            IQueryable<NewsEntry> result = from entry in _db.Entries
                         where entry.Id == Id
                         select entry;
            _db.Entries.Remove(result.First());
            _db.SaveChanges();
            //return RedirectToAction("Index");
            return View();
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult Create(News entry) //Принимает класс в качестве параметра
        {
            //if (image != null)
            //{
            //    byte[] imageData = null;
            //    // считываем переданный файл в массив байтов
            //    using (var binaryReader = new BinaryReader(image.InputStream))
            //    {
            //        imageData = binaryReader.ReadBytes(image.ContentLength);
            //    }
            //    // установка массива байтов
            //    entry.ImageData = imageData;
            //}

            ImagesEntry image = new ImagesEntry();
            NewsEntry entry2 = new NewsEntry();

            entry2.Text = entry.Text;
            entry2.Title = entry.Title;

            entry2.DateAdded = DateTime.Now;
            _db.Entries.Add(entry2); // Сохраняет запись гостевой книги
            _db.SaveChanges();

            image.Type = "news";
            image.Feedback = entry2.Id;

            if (entry.upload1 != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(entry.upload1.FileName);
                // сохраняем файл в папку Files в проекте
                entry.upload1.SaveAs(Server.MapPath("~/Content/Image/News/" + fileName));
                entry.Image1 = image.Image = "Content/Image/News/" + fileName;
                _dbi.Entries.Add(image);
                _dbi.SaveChanges();
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
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
                //entry.upload1 = null;
                entry2.Image1 = entry.Image1;
            }

            _dbi.SaveChanges();

            //return Content("New entry successfully added.");
            return RedirectToAction("SuccessCreate"); // Перенаправляет обратно к действию Index

            // связывание данных модели (Model binding)
        }

        //public ViewResult Show(int id)
        //{
        //    var entry = _db.Entries.Find(id);
        //    bool hasPermission = User.Identity.Name == entry.Title;
        //    ViewData["hasPermission"] = hasPermission;
        //    //ViewBag.HasPermission = hasPermission;
        //    return View(entry);
        //}

        
        //public ViewResult Index(int page = 1)
        //{
        //    return View(ViewBag.Entries
        //      .OrderBy(p => p.Id)
        //      .Skip((page - 1) * PageSize)
        //      .Take(PageSize));
        //}
    }
}



//public class GuestbookController : Controller
//{
//    //
//    // GET: /Guestbook/

//    private GuestbookContext _db = new GuestbookContext();

//    public ActionResult Index()
//    {
//        var mostRecentEntries =        // Получает самые последние записи
//          (from entry in _db.Entries
//           orderby entry.DateAdded descending
//           select entry).Take(20);
//        ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление
//        return View();
//        //var model = mostRecentEntries.ToList();
//        // return View(model);

//    }


//    public ActionResult Create()
//    {
//        return View();
//    }

//    public ViewResult Show(int id)
//    {
//        var entry = _db.Entries.Find(id);
//        bool hasPermission = User.Identity.Name == entry.Name;
//        ViewData["hasPermission"] = hasPermission;
//        //ViewBag.HasPermission = hasPermission;
//        return View(entry);
//    }

//    [HttpPost]   // Ограничивает доступ только через HTTP метод POST
//    public ActionResult Create(GuestbookEntry entry) //Принимает класс GuestbookEntry в качестве параметра
//    {
//        entry.DateAdded = DateTime.Now;
//        _db.Entries.Add(entry); // Сохраняет запись гостевой книги
//        _db.SaveChanges();
//        //return Content("New entry successfully added.");
//        return RedirectToAction("Index"); // Перенаправляет обратно к действию Index

//        // связывание данных модели (Model binding)
//    }
//}