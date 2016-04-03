using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kindergarten.Models;

namespace Kindergarten.Controllers
{
    public class BagController : Controller
    {
        //
        // GET: /Bag/

        private GuestbookContext _db1 = new GuestbookContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]   // Ограничивает доступ только через HTTP метод POST
        public ActionResult Reply(GuestbookEntry entry)
        {
            _db1.Entries.Remove(_db1.Entries.Find(entry.Id));
            _db1.SaveChanges();
            _db1.Entries.Add(entry); // Сохраняет запись гостевой книги
            _db1.SaveChanges();
            return Content("Ответ на вопрос успешно добавлен.");
        }

    }
}
