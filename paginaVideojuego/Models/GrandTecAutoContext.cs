using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace paginaVideojuego.Models
{
    public partial class GrandTecAutoContext : DbContext
    {
        public GrandTecAutoContext()
        {
        }

        public GrandTecAutoContext(DbContextOptions<GrandTecAutoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Partida> Partidas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<PartidaN> PartidasN { get; set; }
        public virtual DbSet<UsuarioM> UsuariosM { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=GrandTecAuto;User Id=admin;Password=hola;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Partida>(entity =>
            {
                entity.HasKey(e => e.IdPartida)
                    .HasName("partidas_pkey");

                entity.ToTable("partidas");

                entity.Property(e => e.IdPartida).HasColumnName("id_partida");

                entity.Property(e => e.DuracionMinutosPartida).HasColumnName("duracion_minutos_partida");

                entity.Property(e => e.FechaPartida)
                    .HasColumnType("date")
                    .HasColumnName("fecha_partida")
                    .HasDefaultValueSql("CURRENT_DATE");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.PuntajePartida).HasColumnName("puntaje_partida");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Partida)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("fk_usuarios");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("usuarios_pkey");

                entity.ToTable("usuarios");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.ClaveUsuario)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("clave_usuario");

                entity.Property(e => e.ContinenteUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("continente_usuario");

                entity.Property(e => e.FechaIngresoUsuario)
                    .HasColumnType("date")
                    .HasColumnName("fecha_ingreso_usuario")
                    .HasDefaultValueSql("CURRENT_DATE");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("nombre_usuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
