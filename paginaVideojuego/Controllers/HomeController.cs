//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using paginaVideojuego.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace paginaVideojuego.Controllers
{
    public class HomeController : Controller
    {
        static List<UsuarioModel> jugadores = new List<UsuarioModel>();

        private readonly GrandTecAutoContext database;

        public HomeController(GrandTecAutoContext database)
        {
            this.database = database;
        }

        public IActionResult Juega()
        {
            return View();
        }

        public IActionResult Instrucciones()
        {
            return View();
        }

        public IActionResult Records()
        {
            var sql = "SELECT * FROM top100_partidas()";
            var result = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();

            return View(result);
        }

        public IActionResult Perfil()
        {
            return View();
        }

        public IActionResult EditarPerfil()
        {
            return View();
        }

        public IActionResult Login()
        {
            //HttpContext.Session.SetString("username", "mar");
            //HttpContext.Session.SetInt32("username", "mar");

            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}