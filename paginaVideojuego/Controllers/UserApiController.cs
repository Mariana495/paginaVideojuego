using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using paginaVideojuego.Models;
using System.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace paginaVideojuego.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserApiController : Controller
    {

        private readonly GrandTecAutoContext database;

        public UserApiController(GrandTecAutoContext database)
        {
            this.database = database;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
        //    {

        //        return Ok("{ idUsuario = 999, nombreUsuario = 'sinsesion' }");
        //    }
        //    else
        //    {
        //        int playerId = Int32.Parse(HttpContext.Session.GetString("IdUsuario"));
        //        var nombreUsuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == playerId);

        //        if (nombreUsuario != null)
        //        {
        //            return Ok("{ idUsuario = 999, nombreUsuario = 'sinsesion' }");
        //        }

        //        return Ok("{ idUsuario = 999, nombreUsuario = 'sinsesion' }");
        //    }
        //}

        [HttpGet]
        public IActionResult Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {

                return Ok(new UsuarioJuego { idUsuario = 999, nombreUsuario = "sinsesion" });
            }
            else
            {
                int playerId = Int32.Parse(HttpContext.Session.GetString("IdUsuario"));
                var nombreUsuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == playerId);

                if (nombreUsuario != null)
                {
                    return Ok(new UsuarioJuego { idUsuario = 999, nombreUsuario = "sinsesion" });
                }

                return Ok(new UsuarioJuego { idUsuario = 999, nombreUsuario = "sinsesion" });
            }
        }

        //[HttpGet]
        //public UsuarioJuego Get()
        //{
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
        //    {

        //        return new UsuarioJuego {idUsuario =  999, nombreUsuario = "sinsesion" }; 
        //    }
        //    else
        //    {
        //        int playerId = Int32.Parse(HttpContext.Session.GetString("IdUsuario")); 
        //        var nombreUsuario = database.Usuarios.SingleOrDefault(x => x.IdUsuario == playerId);

        //        if (nombreUsuario != null) 
        //        {
        //            return new UsuarioJuego { idUsuario = 999, nombreUsuario = "sinsesion" };
        //        }

        //        return new UsuarioJuego { idUsuario = 999, nombreUsuario = "sinsesion" };
        //    }
        //}


        [HttpPost]
        public IActionResult Post([FromBody] PartidaJuego partida)
        {
            database.Database.ExecuteSqlRaw($"call agregar_partida('{partida.puntajePartida}', '{partida.duracionPartida}', '{partida.idUsuario}')");
            return Ok();
        }

        //[HttpPost]
        //public void Post([FromBody] string partidaString)
        //{
        //    PartidaJuego partida = JsonSerializer.Deserialize<PartidaJuego>(partidaString);
        //    database.Database.ExecuteSqlRaw($"call agregar_partida('{partida.puntajePartida}', '{partida.duracionPartida}', '{partida.idUsuario}')");
        //}
    }
}
