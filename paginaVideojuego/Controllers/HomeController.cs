﻿using Microsoft.AspNetCore.Http;
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
        //static List<UsuarioModel> jugadores = new List<UsuarioModel>();

        private readonly GrandTecAutoContext database;
        public readonly ILogger<HomeController> _logger;

        public HomeController(GrandTecAutoContext database, ILogger<HomeController> logger)
        {
            this.database = database;
            _logger = logger;
        }

        public IActionResult Juega()
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            return View();
        }

        public IActionResult Instrucciones()
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            return View();
        }

        public IActionResult Records()
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            var sql = @"SELECT usr.nombre_usuario as nombreusuario, part.puntaje_partida as puntajepartida, part.duracion_minutos_partida as duracionpartida, part.fecha_partida as fechapartida

                        FROM partidas AS part

                        INNER JOIN usuarios AS usr ON part.id_usuario = usr.id_usuario

                        ORDER BY part.puntaje_partida DESC LIMIT 100; ";
            var result = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();

            return View(result);
        }

        public IActionResult Perfil(int id)
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if (usuario == null)
            {
                TempData["Error"] = "No se encontró al usuario";
                return RedirectToAction("Juega");
            }

            var sql = "SELECT * FROM top100_partidas_usuario(id)";

            var result = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();

            return View(result);
        }

        [HttpGet]
        public IActionResult EditarPerfil(int id)
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if(usuario == null)
            {
                TempData["Error"] = "No se encontró al usuario";
                return RedirectToAction("Juega");
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult EditarPerfil(Usuario cambioDatosUsuario)
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Hubo un error en el modelo";
                return View(cambioDatosUsuario);
            }

            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == cambioDatosUsuario.IdUsuario);

            if (usuario == null)
            {
                TempData["Error"] = "No se encontró registro del usuario al editar";
                return RedirectToAction("Juega");
            }

            usuario.NombreUsuario = cambioDatosUsuario.NombreUsuario;
            usuario.ClaveUsuario = cambioDatosUsuario.ClaveUsuario;
            usuario.ContinenteUsuario = cambioDatosUsuario.ContinenteUsuario;

            database.SaveChanges();

            return RedirectToAction("Juega");
        }

        public IActionResult Delete(int id)
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado al borrar";
                return RedirectToAction("Juega");
            }

            database.Usuarios.Remove(usuario);

            database.SaveChanges();

            return RedirectToAction("Juega");
        }

        public IActionResult Login()
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");
            var random = new Random();

            if(HttpContext.Session.GetString("NombreUsuario") == null)
            {
                HttpContext.Session.SetString("NombreUsuario", "CJ" + random.Next().ToString());
            }
            //HttpContext.Session.SetString("username", "mar");
            //HttpContext.Session.SetInt32("username", "mar");

            return View();

        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            ViewData["NombreUsuario"] = HttpContext.Session.GetString("NombreUsuario");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Faltan datos";
                return View(usuario);
            }

            var random = new System.Random();

            usuario.IdUsuario = random.Next();

            database.Usuarios.Add(usuario);

            database.SaveChanges();

            return RedirectToAction("Juega");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}