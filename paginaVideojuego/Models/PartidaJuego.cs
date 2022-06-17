using Microsoft.EntityFrameworkCore;

#nullable disable

namespace paginaVideojuego.Models
{
    [Keyless]
    public class PartidaJuego
    {
        public int idUsuario { get; set; }
        public double puntajePartida { get; set; }
        public double duracionPartida { get; set; }
    }
}

