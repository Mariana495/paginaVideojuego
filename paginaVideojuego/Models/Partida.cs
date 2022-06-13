using System;
using System.Collections.Generic;

#nullable disable

namespace paginaVideojuego.Models
{
    public partial class Partida
    {
        public int IdPartida { get; set; }
        public double PuntajePartida { get; set; }
        public double DuracionMinutosPartida { get; set; }
        public DateTime? FechaPartida { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
