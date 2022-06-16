using Microsoft.EntityFrameworkCore;
using System;
namespace paginaVideojuego.Models
{
    [Keyless]
    public class UsuarioMJ
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public DateTime? FechaIngresoUsuario { get; set; }
        public string ContinenteUsuario { get; set; }
        public double MinutosJugados { get; set; }
    }
}
