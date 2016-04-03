using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kindergarten.Models;

namespace Kindergarten.Controllers
{
    public class GuestbookController : Controller
    {
        //
        // GET: /Guestbook/

        private GuestbookContext _db = new GuestbookContext();

        public int PageSize = 1;
        public ActionResult Index(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries where entry.Access == 0
               orderby entry.DateAdded descending
               select entry).Skip((page - 1) * PageSize).Take(PageSize);
            ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление
            

            GuestbookListViewModel model = new GuestbookListViewModel
            {
                Entries = ViewBag.Entries,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = (from entry in _db.Entries where entry.Access == 0 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Delete(int Id)
        {
            GuestbookContext _db = new GuestbookContext();
            IQueryable<GuestbookEntry> result = from entry in _db.Entries
                                           where entry.Id == Id
                                           select entry;
            _db.Entries.Remove(result.First());
            _db.SaveChanges();
            //return RedirectToAction("Index");
            return View();
        }

        public ActionResult DeleteAnswer(int Id)
        {
            GuestbookContext _db = new GuestbookContext();
            var mostRecentEntries =        // Получает самые последние записи
             from entry in _db.Entries
             where entry.Id == Id
             select entry;
            ViewBag.Entries = mostRecentEntries.ToList();

            GuestbookEntry temp = new GuestbookEntry();
            temp.Id = ViewBag.Entries[0].Id;
            temp.DateAdded = ViewBag.Entries[0].DateAdded;
            temp.Name = ViewBag.Entries[0].Name;
            temp.Message = ViewBag.Entries[0].Message;
            temp.Access = ViewBag.Entries[0].Access;

            _db.Entries.Remove(_db.Entries.Find(temp.Id));
            _db.SaveChanges();
            _db.Entries.Add(temp); // Сохраняет запись гостевой книги
            _db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ChangeAnswer(int Id/*, int i*/)
        {
            var mostRecentEntries =        // Получает самые последние записи
              from entry in _db.Entries
              where entry.Id == Id
              select entry;
            ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление
            GuestbookEntry model = new GuestbookEntry
            {
                Id = ViewBag.Entries[0].Id,
                Name = ViewBag.Entries[0].Name,
                Message = ViewBag.Entries[0].Message,
                DateAdded = ViewBag.Entries[0].DateAdded,
                Reply = ViewBag.Entries[0].Reply,
                Access = ViewBag.Entries[0].Access
            };
            //var model = from entry in _db.Entries
            //                     where entry.Id == Id
            //                     select entry;
            return View(model);
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult ChangeAnswer(GuestbookEntry entry)
        {
            entry.DateAdded = _db.Entries.Find(entry.Id).DateAdded;
            entry.Name = _db.Entries.Find(entry.Id).Name;
            entry.Message = _db.Entries.Find(entry.Id).Message;
            entry.Access = _db.Entries.Find(entry.Id).Access;
            _db.Entries.Remove(_db.Entries.Find(entry.Id));
            _db.SaveChanges();
            _db.Entries.Add(entry); // Сохраняет запись гостевой книги
            _db.SaveChanges();
            return RedirectToAction("Index"); // Перенаправляет обратно к действию Index
            //return Content("Ответ на вопрос успешно добавлен.");
        }

        public ActionResult ChangeQuestion(int Id/*, int i*/)
        {
            var mostRecentEntries =        // Получает самые последние записи
              from entry in _db.Entries where entry.Id == Id
               select entry;
            ViewBag.Entries = mostRecentEntries.ToList(); // Передает записи в представление
            GuestbookEntry model = new GuestbookEntry
            {
                Id = ViewBag.Entries[0].Id,
                Name = ViewBag.Entries[0].Name,
                Message = ViewBag.Entries[0].Message,
                DateAdded = ViewBag.Entries[0].DateAdded,
                Reply = ViewBag.Entries[0].Reply,
                Access = ViewBag.Entries[0].Access
            };
            //var model = from entry in _db.Entries
            //                     where entry.Id == Id
            //                     select entry;
            return View(model);
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult ChangeQuestion(GuestbookEntry entry)
        {
            entry.DateAdded = _db.Entries.Find(entry.Id).DateAdded;
            //entry.Name = _db.Entries.Find(entry.Id).Name;
            //entry.Message = _db.Entries.Find(entry.Id).Message;
            entry.Access = _db.Entries.Find(entry.Id).Access;
            entry.Reply = _db.Entries.Find(entry.Id).Reply;
            _db.Entries.Remove(_db.Entries.Find(entry.Id));
            _db.SaveChanges();
            _db.Entries.Add(entry); // Сохраняет запись гостевой книги
            _db.SaveChanges();
            return RedirectToAction("Index"); // Перенаправляет обратно к действию Index
            //return Content("Ответ на вопрос успешно добавлен.");
        }

        public ActionResult Reply(int Id/*, int i*/)
        {
            //var model = from entry in _db.Entries
            //                     where entry.Id == Id
            //                     select entry;
            return View(/*model*/);
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult Reply(GuestbookEntry entry)
        {
            entry.DateAdded = _db.Entries.Find(entry.Id).DateAdded;
            entry.Name = _db.Entries.Find(entry.Id).Name;
            entry.Message = _db.Entries.Find(entry.Id).Message;
            entry.Access = _db.Entries.Find(entry.Id).Access;
            _db.Entries.Remove(_db.Entries.Find(entry.Id));
            _db.SaveChanges();
            _db.Entries.Add(entry); // Сохраняет запись гостевой книги
            _db.SaveChanges();
            return RedirectToAction("Index"); // Перенаправляет обратно к действию Index
            //return Content("Ответ на вопрос успешно добавлен.");
        }

        //public ViewResult Show(int id)
        //{
        //    var entry = _db.Entries.Find(id);
        //    bool hasPermission = User.Identity.Name == entry.Name;
        //    ViewData["hasPermission"] = hasPermission;
        //    //ViewBag.HasPermission = hasPermission;
        //    return View(entry);
        //}

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult Create(GuestbookEntry entry) //Принимает класс GuestbookEntry в качестве параметра
        {
            entry.DateAdded = DateTime.Now;
            _db.Entries.Add(entry); // Сохраняет запись гостевой книги
            _db.SaveChanges();
            //return Content("Ваш вопрос успешно добавлен.");
            return RedirectToAction("Index"); // Перенаправляет обратно к действию Index

           // связывание данных модели (Model binding)
        }
    }
}
