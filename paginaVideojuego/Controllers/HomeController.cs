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

            Perfil perfil = new Perfil();

            var sql = "SELECT usr.id_usuario, usr.nombre_usuario, usr.clave_usuario, usr.fecha_ingreso_usuario, usr.continente_usuario, (SELECT coalesce(sum(part.duracion_minutos_partida), 0) FROM partidas AS part WHERE part.id_usuario = " + ViewData["IdUsuario"] + ") as minutosjugados FROM usuarios as usr WHERE usr.id_usuario = " + ViewData["IdUsuario"] + ";";

            var usuariosconminutos = database.UsuariosMJ.FromSqlRaw<UsuarioMJ>(sql).ToList();

            var sql2 = "SELECT * FROM partidas WHERE id_usuario = " + ViewData["IdUsuario"] + " ORDER BY partidas.puntaje_partida DESC LIMIT 100;";
            

            var minutos = database.Partidas.FromSqlRaw<Partida>(sql2).ToList();

            if (usuariosconminutos == null || usuariosconminutos.Count < 1)
            {
                ViewData["Error"] = "No se encontró al usuario";
                return View("Login");
            }

            perfil.UsuarioRegistrado = usuariosconminutos[0];

            perfil.PartidasJugadas = minutos;

            return View(perfil);
        }

        [HttpGet]
        public IActionResult EditarPerfil(int id)
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if(usuario == null)
            {
                ViewData["Error"] = "Usuario Invalido";
                return View("Login");
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult EditarPerfil(Usuario cambioDatosUsuario)
        {
            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");
           
            if (!ModelState.IsValid)
            {
                ViewData["ErrorEdicion"] = "Hubo un error en el modelo";
                return View("Juega");
            }

            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == cambioDatosUsuario.IdUsuario);

            if (usuario == null || usuario.NombreUsuario.Length > 10)
            {
                if (usuario.NombreUsuario.Length > 10)
                {
                    ViewData["ErrorEdicion"] = "Tu nombre debe de tener menos de 11 caracteres";
                    return View("Juega");
                }
                ViewData["ErrorEdicion"] = "No se encontró registro del usuario al editar";
                return View("Juega");
            }

            if (cambioDatosUsuario.ClaveUsuario == null || cambioDatosUsuario.ClaveUsuario.Trim().Length == 0)
            {
                ViewData["ErrorEdicion"] = "Ingresa una contraseña";
                return View("Juega");
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
                ViewData["Error"] = "Usuario no encontrado al borrar";
                return View("Login");
            }

            var sql = $"call eliminar_usuario('{usuario.IdUsuario}')";

            database.Database.ExecuteSqlRaw($"call eliminar_usuario('{usuario.IdUsuario}')");
            database.Database.ExecuteSqlRaw($"call eliminar_partidas_usuario('{usuario.IdUsuario}')");

            //database.Usuarios.Remove(usuario);
            //database.SaveChanges();

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
            if (usuario == null || usuario.NombreUsuario == null || usuario.NombreUsuario.Trim().Length == 0 || usuario.NombreUsuario.Length > 10)
            {
                
                ViewData["ErrorCreacion"] = "Nombre de usuario no registrado";
                return View("Login");
            }

            if (usuario.NombreUsuario.Length > 10)
            {
                ViewData["ErrorCreacion"] = "Tu nombre debe de tener menos de 11 caracteres";
                return View("Login");
            }

            if (usuario.ClaveUsuario == null || usuario.ClaveUsuario.Trim().Length == 0)
            {
                ViewData["ErrorCreacion"] = "Ingrese una contraseña";
                return View("Login");
            }

            if (usuario.ContinenteUsuario == null || usuario.ContinenteUsuario.Trim().Length == 0)
            {
                ViewData["ErrorCreacion"] = "Continente de usuario no registrado";
                return View("Login");
            }

            if (database.Usuarios.SingleOrDefault(x => x.NombreUsuario == usuario.NombreUsuario) != null)
            {
                ViewData["ErrorCreacion"] = "El nombre de usuario ya existe";
                return View("Login");
            }

            // var sql = $"call agregar_usuario('{usuario.NombreUsuario}, {usuario.ClaveUsuario}, {usuario.ContinenteUsuario}')";

            database.Usuarios.Add(usuario);
            database.SaveChanges();

            HttpContext.Session.SetString("IdUsuario", $"('{usuario.IdUsuario}')");

            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

            return RedirectToAction("Juega");
        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            
            var usuario_id = database.Usuarios.SingleOrDefault(x => x.NombreUsuario == usuario.NombreUsuario && x.ClaveUsuario == usuario.ClaveUsuario);
                
            if (usuario_id == null)
            {
                ViewData["Error"] = "No coinciden los datos con el registro";
                return View(usuario);
            }
                
            HttpContext.Session.SetString("IdUsuario", $"('{usuario_id.IdUsuario}')");

            ViewData["IdUsuario"] = HttpContext.Session.GetString("IdUsuario");

            return RedirectToAction("Perfil");

        }

        public IActionResult Salir()
        {
            HttpContext.Session.Clear();

            return View("Juega");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}