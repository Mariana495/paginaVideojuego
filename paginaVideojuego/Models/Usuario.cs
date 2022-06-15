using System;
using System.Collections.Generic;

#nullable disable

namespace paginaVideojuego.Models
{
    public partial class Usuario
    {
        
        public Usuario()
        {
            Partida = new HashSet<Partida>();
        }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public DateTime? FechaIngresoUsuario { get; set; }
        public string ContinenteUsuario { get; set; }

        public virtual ICollection<Partida> Partida { get; set; }

        //public static bool CreacionUsuarioValido(Usuario usuario )
        //{
        //    if (usuario == null || usuario.NombreUsuario.Length >= 10)

        //        return false;
        //    else if (database.Usuarios.SingleOrDefault(x => x.NombreUsuario == usuario.NombreUsuario) == null)
        //        return true;

        //    return true;
        //}
    }
}
