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

        public const int EMPTY_NAME = 100;
        public const int EMPTY_PASSWORD = 101;
        public const int EMPTY_CONTINENT = 102;
        public const int EXISTENT_NAME = 103;
        public const int MISMATCHED_IDS = 104;

        public UserApiController(GrandTecAutoContext database)
        {
            this.database = database;
        }

        public string Get()
        {
            var NombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            if(NombreUsuario == null)
            {
                return "no-session";
            }

            return NombreUsuario;
        }

        [HttpGet]
        public IEnumerable<Usuario> Records()
        {
            return database.Usuarios.ToList();
        }

        [HttpGet("{IdUsuario}")]
        public IActionResult Perfil(int id)
        {
            var usuario = database.Usuarios.SingleOrDefault(row => row.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: UserApiController/Create
        [HttpPost]
        public IActionResult Create([FromBody] Usuario usuario)
        {
            if (usuario == null || usuario.NombreUsuario == null || usuario.NombreUsuario.Trim().Length == 0)
            {
                return BadRequest(new
                {
                    error = "Empty name",
                    code = EMPTY_NAME
                });
            }

            if (usuario.ClaveUsuario == null || usuario.ClaveUsuario.Trim().Length == 0)
            {
                return BadRequest(new
                {
                    error = "Empty password",
                    code = EMPTY_PASSWORD
                });
            }

            if (usuario.ContinenteUsuario == null || usuario.ContinenteUsuario.Trim().Length == 0)
            {
                return BadRequest(new
                {
                    error = "Empty continent",
                    code = EMPTY_CONTINENT
                });
            }

            

            var sql = $"call agregar_usuario('{usuario.NombreUsuario}, {usuario.ClaveUsuario}, {usuario.ContinenteUsuario}')";

            var random = new System.Random();

            usuario.IdUsuario = random.Next();

            database.Usuarios.Add(usuario);

            database.SaveChanges();

            return Ok();
        }

        // PUT: UserApiController/Edit/5
        [HttpPut("{IdUsuario}")]
        public IActionResult Edit(int id, [FromBody] Usuario editarUsuario)
        {
            if (editarUsuario.IdUsuario != id)
            {
                return BadRequest(new
                {
                    error = "Las Id no coinciden",
                    code = MISMATCHED_IDS
                });
            }

            var usuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            if (usuario.NombreUsuario == null || usuario.NombreUsuario.Trim().Length == 0)
            {
                return BadRequest(new
                {
                    error = "Empty name",
                    code = EMPTY_NAME
                });
            }

            if (usuario.ClaveUsuario == null || usuario.ClaveUsuario.Trim().Length == 0)
            {
                return BadRequest(new
                {
                    error = "Empty password",
                    code = EMPTY_PASSWORD
                });
            }

            if (usuario.ContinenteUsuario == null || usuario.ContinenteUsuario.Trim().Length == 0)
            {
                return BadRequest(new
                {
                    error = "Empty continent",
                    code = EMPTY_CONTINENT
                });
            }
            
            usuario.NombreUsuario = editarUsuario.NombreUsuario;
            usuario.ClaveUsuario = editarUsuario.ClaveUsuario;
            usuario.ContinenteUsuario = editarUsuario.ContinenteUsuario;

            var sql = @$"call editar_nombre('{id}, {editarUsuario.NombreUsuario}')
                        call editar_clave('{id}, {editarUsuario.ClaveUsuario}')
                        call editar_continente('{id}, {editarUsuario.ContinenteUsuario}')";

            database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{IdUsuario}")]
        public IActionResult Delete(int id, [FromBody] Usuario usuario)
        {
            var usuarios = database.Usuarios.SingleOrDefault(x => x.IdUsuario == id);

            if (usuarios == null)
            {
                return NotFound();
            }

            var sql = @$"call eliminar_partidas_usuario('{id}')
                        call eliminar_usuario('{usuario.NombreUsuario}')";

            database.Usuarios.Remove(usuarios);

            database.SaveChanges();

            return Ok();
        }

    }
}
