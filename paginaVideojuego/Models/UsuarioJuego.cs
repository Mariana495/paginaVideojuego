using Microsoft.EntityFrameworkCore;

#nullable disable

namespace paginaVideojuego.Models
{
    [Keyless]
    public class UsuarioJuego
    {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
    }
}
