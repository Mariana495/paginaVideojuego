using Microsoft.EntityFrameworkCore;

#nullable disable

namespace paginaVideojuego.Models
{
    [Keyless]
    public class PartidaJuego
    {
        public int idUsuario { get; set; }
        public float puntajePartida { get; set; }
        public float duracionPartida { get; set; }
    }
}

