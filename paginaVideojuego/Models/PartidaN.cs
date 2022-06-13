using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace paginaVideojuego.Models
{
    [Keyless]
    public class PartidaN
    {
        public string NombreUsuario { get; set; }
        public double PuntajePartida { get; set; }
        public double DuracionMinutosPartida { get; set; }
        public DateTime? FechaPartida { get; set; }

    }
}
