using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kindergarten.Models;

namespace Kindergarten.Controllers
{
    public class ChildrenController : Controller
    {
        //
        // GET: /Children/

        private GuestbookContext _db = new GuestbookContext();

        public int PageSize = 1;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
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

        public ActionResult Gnomiki(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries where entry.Access == 1  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 1 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Korabliki(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == 2  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 2 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Rodnichok(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == 3  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 3 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Semeyka(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == 4  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 4 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Solnyshko(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == 5  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 5 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Teremok(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == 6  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 6 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Zemlyanichki(int page = 1)
        {
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == 7  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == 7 select entry).Count()
                }
            };

            return View(model);
        }

        public ActionResult Group(int access)
        {
            int page = 1;
            var mostRecentEntries =        // Получает самые последние записи
              (from entry in _db.Entries
               where entry.Access == access  // только разрешенные к чтению 
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
                    TotalItems = (from entry in _db.Entries where entry.Access == access select entry).Count(),
                    access = access
                }
            };

            return View(model);
        }

    }
}
