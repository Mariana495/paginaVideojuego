using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace paginaVideojuego.Controllers
{
    public class UserApiController : Controller
    {
        // GET: UserApiController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserApiController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserApiController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserApiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserApiController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserApiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserApiController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserApiController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
