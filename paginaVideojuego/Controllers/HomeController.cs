using Microsoft.AspNetCore.Http;
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
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
            return View();
        }

        public IActionResult Instrucciones()
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
            return View();
        }

        public IActionResult Records()
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
            var sql = @"SELECT usr.nombre_usuario as nombreusuario, part.puntaje_partida as puntajepartida, part.duracion_minutos_partida as duracionpartida, part.fecha_partida as fechapartida

                        FROM partidas AS part

                        INNER JOIN usuarios AS usr ON part.id_usuario = usr.id_usuario

                        ORDER BY part.puntaje_partida DESC LIMIT 100; ";
            var result = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();

            return View(result);
        }

        public IActionResult Perfil()
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

            var sql = $"SELECT * FROM informacion_usuario(" + ViewData["IdUsuario"] + ")";
            
            var result = database.Usuarios.FromSqlRaw<Usuario>(sql).ToList();

            if (result == null || result.Count < 1)
            {
                TempData["Error"] = "No se encontró al usuario";
                return RedirectToAction("Juega");
            }

            return View(result[0]);
        }

        public IActionResult Resultados(int id)
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
            
            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if (usuario == null)
            {
                TempData["Error"] = "No se encontró al usuario";
                return RedirectToAction("Juega");
            }

            var sql = "SELECT * FROM top100_partidas_usuario(id)";

            var result = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();;

            return View(result);
        }

        [HttpGet]
        public IActionResult EditarPerfil(int id)
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

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
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
           
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
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
           
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
  
            return View();

        }

        [HttpPost]
        public IActionResult Login(Usuario usuario/*, bool iniciar_sesion*/)
        {
            

            /*if (iniciar_sesion == true)
            {*/

            var usuario_id = database.Usuarios.SingleOrDefault(x => x.NombreUsuario == usuario.NombreUsuario && x.ClaveUsuario == usuario.ClaveUsuario);
                
                if (usuario_id == null)
                {
                    TempData["Error"] = "No coinciden los datos con el registro";
                    return View(usuario);
                }

                HttpContext.Session.SetString("IdUsuario", $"('{usuario_id.IdUsuario}')");

                ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

                return RedirectToAction("Juega");
            /*}

            else
            {
                var usuario_nombre = database.Usuarios.SingleOrDefault(x => x.NombreUsuario == usuario.NombreUsuario);

                if (usuario == null)
                {
                    var sql = $"call agregar_usuario('{usuario.NombreUsuario}, {usuario.ClaveUsuario}, {usuario.ContinenteUsuario}')";
                }

                

                return RedirectToAction("Juega");
            }*/
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}