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

        public const int EMPTY_NAME = 100;
        public const int EMPTY_PASSWORD = 101;
        public const int EMPTY_CONTINENT = 102;
        public const int EXISTENT_NAME = 103;
        public const int MISMATCHED_IDS = 104;

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
            var records = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();

            return View(records);
        }

        public IActionResult Perfil()
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

            //var datos = int.Parse(HttpContext.Session.GetString("IdUsuario"));

            var sql = @"SELECT usr.id_usuario, usr.nombre_usuario, usr.clave_usuario, usr.fecha_ingreso_usuario, usr.continente_usuario, (SELECT sum(part.duracion_minutos_partida) FROM partidas AS part WHERE part.id_usuario = " + ViewData["IdUsuario"] + ") as minutosjugados FROM usuarios as usr WHERE usr.id_usuario = " + ViewData["IdUsuario"] + ";";
                //SELECT * FROM informacion_usuario (" + ViewData["IdUsuario"] + ")";

            
            var data = database.Usuarios.FromSqlRaw<Usuario>(sql).ToList();

            if (data == null || data.Count < 1)
            {
                TempData["Error"] = "No se encontró al usuario";
                return RedirectToAction("Juega");
            }

            return View(data[0]);
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

            var sql = "SELECT usr.nombre_usuario as nombreusuario, part.puntaje_partida as puntajepartida, part.duracion_minutos_partida as duracionpartida, part.fecha_partida as fechapartida FROM partidas AS part INNER JOIN usuarios AS usr ON part.id_usuario = usr.id_usuario WHERE usr.id_usuario = " + ViewData["IdUsuario"] + " ORDER BY part.puntaje_partida DESC LIMIT 100;";

            var results = database.PartidasN.FromSqlRaw<PartidaN>(sql).ToList();;

            return View(results);
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

            var sql = @$"call editar_nombre('{usuario.IdUsuario}, {cambioDatosUsuario.NombreUsuario}')
                        call editar_clave('{usuario.IdUsuario}, {cambioDatosUsuario.ClaveUsuario}')
                        call editar_continente('{usuario.IdUsuario}, {cambioDatosUsuario.ContinenteUsuario}')";

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

            var sql = @$"call eliminar_partidas_usuario('{usuario.IdUsuario}')
                        call eliminar_usuario('{usuario.NombreUsuario}')";

            database.Usuarios.Remove(usuario);

            database.SaveChanges();

            HttpContext.Session.Clear();

            return RedirectToAction("Juega");
        }

        public IActionResult Login()
        {
  
            return View();

        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (usuario == null || usuario.NombreUsuario == null || usuario.NombreUsuario.Trim().Length == 0)
            {
                TempData["Error"] = "Nombre de usuario no registrado";
                return RedirectToAction("Login");
            }
            

            if (usuario.ClaveUsuario == null || usuario.ClaveUsuario.Trim().Length == 0)
            {
                TempData["Error"] = "Clave de usuario no registrado";
                return RedirectToAction("Login");
            }

            if (usuario.ContinenteUsuario == null || usuario.ContinenteUsuario.Trim().Length == 0)
            {
                TempData["Error"] = "Continente de usuario no registrado";
                return RedirectToAction("Login");
            }



            var sql = $"call agregar_usuario('{usuario.NombreUsuario}, {usuario.ClaveUsuario}, {usuario.ContinenteUsuario}')";

            database.Usuarios.Add(usuario);

            database.SaveChanges();

            HttpContext.Session.SetString("IdUsuario", $"('{usuario.IdUsuario}')");

            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

            return RedirectToAction("Juega");
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

                return RedirectToAction("Perfil");
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