using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace paginaVideojuego.Models
{
    [Keyless]
    public class Perfil
    {
        public UsuarioMJ UsuarioRegistrado { get; set; }
        public List<Partida> PartidasJugadas { get; set;}
    }
}
