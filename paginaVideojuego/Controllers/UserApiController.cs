using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using paginaVideojuego.Models;
using System.Linq;

namespace paginaVideojuego.Controllers
{
    public class UserApiController : Controller
    {
        // GET: UserApiController
        static List<UsuarioModel> jugadores = new List<UsuarioModel>();
        
        private readonly GrandTecAutoContext database;

        public UserApiController(GrandTecAutoContext database)
        {
            this.database = database;
        }

        public ActionResult Juega()
        {
            return View();
        }

        // GET: UserApiController/Details/5
        public ActionResult Instrucciones()
        {
            return View();
        }

        // GET: UserApiController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Records()
        {
            var users = database.Usuarios.ToList();

            return View(users);
        }

        // POST: UserApiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Juega));
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
                return RedirectToAction(nameof(Juega));
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
                return RedirectToAction(nameof(Juega));
            }
            catch
            {
                return View();
            }
        }
    }
}
