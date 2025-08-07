using EMSI_Corporation.Models;
using Microsoft.EntityFrameworkCore;


namespace EMSI_Corporation.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Empleado> empleados { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<User_Rol> UserRoles { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Rol_Menu> RolMenus { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Comprobante> Comprobantes { get; set; }
        public DbSet<Extintor> Extintores { get; set; }
        public DbSet<ComprobanteServicio> ComprobantesServicio { get; set; }
        public DbSet<VisitaTecnica> VisitasTecnicas { get; set; }
        public DbSet<InformeTecnico> InformesTecnicos { get; set; }
        public DbSet<Mantenimiento> Mantenimientos { get; set; }
        public DbSet<Recarga> Recargas { get; set; }
        public DbSet<ReporteServicio> ReportesServicio { get; set; }
        public DbSet<Recepcion> Recepcion { get; set; }
        public DbSet<Proovedor> Provedor { get; set; }
        public DbSet<EventoCalendario> EventosCalendario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>(tb =>
            {
                tb.HasKey(e => e.IdEmpleado);
                tb.Property(e => e.IdEmpleado).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(e => e.nomEmpleado).HasMaxLength(100).IsRequired();
                tb.Property(e => e.apeEmpleado).HasMaxLength(100).IsRequired();
                tb.Property(e => e.dni).HasMaxLength(8).IsRequired();
                tb.Property(e => e.gmail).HasMaxLength(100).IsRequired();
                tb.Property(e => e.numCelular).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(u => u.IdUsuario);
                tb.Property(u => u.IdUsuario).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(u => u.usuario).HasMaxLength(100).IsRequired();
                tb.Property(u => u.password).HasMaxLength(100).IsRequired();

                tb.HasOne(u => u.empleado)
                    .WithOne(e => e.usuario)
                    .HasForeignKey<Usuario>(u => u.IdEmpleado)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User_Rol>(tb =>
            {
                tb.HasKey(ur => new
                {
                    ur.IdUsuario,
                    ur.IdRol
                });

                tb.HasOne(ur => ur.Usuario)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.IdUsuario);

                tb.HasOne(ur => ur.Rol)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.IdRol);

            });

            modelBuilder.Entity<Rol>(tb =>
            {
                tb.HasKey(r => r.IdRol);
                tb.Property(r => r.IdRol).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(r => r.nomRol).HasMaxLength(50).IsRequired();
                tb.Property(r => r.Descripcion).HasMaxLength(250).IsRequired();
            });
            modelBuilder.Entity<EventoCalendario>(tb =>
            {
                tb.HasKey(e => e.IdEvento);
                tb.Property(e => e.IdEvento)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(e => e.Titulo)
                    .HasMaxLength(150)
                    .IsRequired();

                tb.Property(e => e.Descripcion)
                    .HasMaxLength(500);

                tb.Property(e => e.FechaInicio)
                    .IsRequired();

                tb.Property(e => e.FechaFin)
                    .IsRequired(false);

                tb.Property(e => e.Color)
                    .HasMaxLength(20);

                tb.HasOne(e => e.Empleado)
                    .WithMany()
                    .HasForeignKey(e => e.EmpleadoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Rol_Menu>(tb =>
            {
                tb.HasKey(ur => new
                {
                    ur.IdMenu,
                    ur.IdRol
                });

                tb.HasOne(ur => ur.Rol)
                    .WithMany(u => u.RolMenus)
                    .HasForeignKey(ur => ur.IdRol);

                tb.HasOne(ur => ur.Menu)
                    .WithMany(u => u.RolMenus)
                    .HasForeignKey(ur => ur.IdMenu);
            });

            modelBuilder.Entity<Menu>(tb =>
            {
                tb.HasKey(m => m.IdMenu);
                tb.Property(m => m.IdMenu).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(m => m.nomMenu).HasMaxLength(50).IsRequired();
                tb.Property(m => m.Url).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<Cliente>(tb =>
            {
                tb.HasKey(c => c.IdCliente);
                tb.Property(c => c.IdCliente).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(c => c.NomCliente).HasMaxLength(100).IsRequired();
                tb.Property(c => c.TipoCliente).HasMaxLength(50).IsRequired();
                tb.Property(c => c.TipoDocumento).HasMaxLength(50).IsRequired();
                tb.Property(c => c.NumDocumento).HasMaxLength(20).IsRequired();
                tb.Property(c => c.NumCelular).HasMaxLength(20).IsRequired();
                tb.Property(c => c.Correo).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Venta>(tb =>
            {
                tb.HasKey(v => v.IdVenta);
                tb.Property(v => v.IdVenta).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(v => v.FechaVenta).IsRequired();
                tb.Property(v => v.MetodoPago).HasMaxLength(50).IsRequired();
                tb.Property(v => v.Total).HasColumnType("decimal(10,2)").IsRequired();

                tb.HasOne(v => v.Cliente)
                    .WithMany()
                    .HasForeignKey(v => v.Cliente_ID);

                tb.HasOne(v => v.Empleado)
                    .WithMany()
                    .HasForeignKey(v => v.Empleado_ID);
            });

            modelBuilder.Entity<Comprobante>(tb =>
            {
                tb.HasKey(c => c.IdComprobante);
                tb.Property(c => c.IdComprobante).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(c => c.TipoComprobante).HasMaxLength(50).IsRequired();
                tb.Property(c => c.NroComprobante).HasMaxLength(100).IsRequired();
                tb.Property(c => c.MontoTotal).HasColumnType("decimal(10,2)").IsRequired();

                tb.HasOne(c => c.Venta)
                    .WithMany()
                    .HasForeignKey(c => c.Venta_ID);
            });

            modelBuilder.Entity<Extintor>(tb =>
            {
                tb.HasKey(e => e.IdExtintor);
                tb.Property(e => e.IdExtintor).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(e => e.TipoAgente).HasMaxLength(50).IsRequired();
                tb.Property(e => e.ClaseFuego).HasMaxLength(50).IsRequired();
                tb.Property(e => e.CapacidadKG).HasColumnType("decimal(5,2)").IsRequired();
                tb.Property(e => e.CantidadDisponible).IsRequired();
                tb.Property(e => e.FechaVencimiento).IsRequired();
                tb.Property(e => e.Estado).HasMaxLength(50).IsRequired();

                tb.HasOne(e => e.Venta)
                    .WithMany()
                    .HasForeignKey(e => e.Venta_ID);
            });

            modelBuilder.Entity<ComprobanteServicio>(tb =>
            {
                tb.HasKey(cs => cs.IdComprobante);
                tb.Property(cs => cs.IdComprobante).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(cs => cs.TipoServicio).HasMaxLength(50).IsRequired();
                tb.Property(cs => cs.Cantidad).IsRequired();
                tb.Property(cs => cs.PrecioUnitario).HasColumnType("decimal(10,2)").IsRequired();
                tb.Property(cs => cs.SubTotal).HasColumnType("decimal(10,2)").IsRequired();
            });

            modelBuilder.Entity<VisitaTecnica>(tb =>
            {
                tb.HasKey(vt => vt.IdVisita);
                tb.Property(vt => vt.IdVisita).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(vt => vt.Fecha).IsRequired();
                tb.Property(vt => vt.Observaciones).IsRequired();

                tb.HasOne(vt => vt.Empleado)
                    .WithMany()
                    .HasForeignKey(vt => vt.Empleado_ID);

                tb.HasOne(vt => vt.Cliente)
                    .WithMany()
                    .HasForeignKey(vt => vt.Cliente_ID);
            });

            modelBuilder.Entity<InformeTecnico>(tb =>
            {
                tb.HasKey(it => it.IdInforme);
                tb.Property(it => it.IdInforme).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(it => it.FechaInforme).IsRequired();
                tb.Property(it => it.Informe_PDF).HasMaxLength(255).IsRequired();

                tb.HasOne(it => it.VisitaTecnica)
                    .WithMany()
                    .HasForeignKey(it => it.Visita_ID);
            });

            modelBuilder.Entity<Mantenimiento>(tb =>
            {
                tb.HasKey(m => m.IdMantenimiento);
                tb.Property(m => m.IdMantenimiento).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(m => m.FechaMantenimiento).IsRequired();
                tb.Property(m => m.LugarAdecuado).IsRequired();
                tb.Property(m => m.Señalización).IsRequired();
                tb.Property(m => m.Visible).IsRequired();
                tb.Property(m => m.Usado).IsRequired();
                tb.Property(m => m.EstadoPrecinto).IsRequired();
                tb.Property(m => m.EstadoIndicador).IsRequired();
                tb.Property(m => m.PresionCorrecta).IsRequired();
                tb.Property(m => m.ExteriorCorrecto).IsRequired();
                tb.Property(m => m.PesoCorrecto).IsRequired();
                tb.Property(m => m.MangueraCorrecta).IsRequired();
                tb.Property(m => m.BoquillaCorrecta).IsRequired();
                tb.Property(m => m.InstruccionesVisibles).IsRequired();
                tb.Property(m => m.AperturaCorrecta).IsRequired();
                tb.Property(m => m.BarometroCorrecto).IsRequired();

                tb.HasOne(m => m.Empleado)
                    .WithMany()
                    .HasForeignKey(m => m.Empleado_ID);

                tb.HasOne(m => m.Extintor)
                    .WithMany()
                    .HasForeignKey(m => m.Extintor_ID).OnDelete(DeleteBehavior.NoAction);
                tb.HasOne(m => m.ReporteServicio)
                    .WithMany()
                    .HasForeignKey(m => m.ReporteServicio_ID).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Recarga>(tb =>
            {
                tb.HasKey(r => r.IdRecarga);
                tb.Property(r => r.IdRecarga).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(r => r.FechaRecarga).IsRequired();
                tb.Property(r => r.MaterialUsado).HasMaxLength(100).IsRequired();
                tb.Property(r => r.CantidadKG).HasColumnType("decimal(5,2)").IsRequired();
                tb.Property(r => r.PresionPostRecarga).HasColumnType("decimal(5,2)").IsRequired();

                tb.HasOne(r => r.Extintor)
                    .WithMany()
                    .HasForeignKey(r => r.Extintor_ID).OnDelete(DeleteBehavior.NoAction);

                tb.HasOne(r => r.Empleado)
                    .WithMany()
                    .HasForeignKey(r => r.Empleado_ID);
                tb.HasOne(m => m.ReporteServicio)
                    .WithMany()
                    .HasForeignKey(m => m.ReporteServicio_ID).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ReporteServicio>(tb =>
            {
                tb.HasKey(rs => rs.IdReporte);
                tb.Property(rs => rs.IdReporte).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(rs => rs.FirmaCliente).HasColumnType("bytea");
                tb.Property(rs => rs.Observaciones).IsRequired();
                tb.Property(rs => rs.ImgEvidencia).HasColumnType("bytea");

                tb.HasOne(rs => rs.Cliente)
                    .WithMany()
                    .HasForeignKey(rs => rs.Cliente_ID);

                tb.HasOne(rs => rs.ComprobanteServicio)
                .WithMany()
                .HasForeignKey(rs => rs.Comprobante_ID);
            });

            modelBuilder.Entity<Recepcion>(tb =>
            {
                tb.HasKey(rp => rp.IdRecepcion);
                tb.Property(rp => rp.IdRecepcion).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(rp => rp.Fecha).IsRequired();

                // Relación con Usuario
                tb.HasOne(r => r.Usuario)
                  .WithMany(u => u.Recepciones)
                  .HasForeignKey(r => r.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);

                // Relación con Proovedor
                tb.HasOne(r => r.Proovedor)
                  .WithMany(p => p.Recepciones)
                  .HasForeignKey(r => r.ProovedorId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Proovedor>(tb =>
            {
                tb.HasKey(pro => pro.IdProovedor);
                tb.Property(pro => pro.RazonSocial).HasMaxLength(100).IsRequired();
                tb.Property(pro => pro.RUC).HasMaxLength(11).IsRequired();
                tb.Property(pro => pro.Direccion).HasMaxLength(150);
                tb.Property(pro => pro.NumCelular).HasMaxLength(15);
                tb.Property(pro => pro.Correo).HasMaxLength(100);
                tb.Property(pro => pro.estado).IsRequired();
            });


            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<User_Rol>().ToTable("User_Rol");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<Rol_Menu>().ToTable("Rol_Menu");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<Recepcion>().ToTable("Recepcion");
            modelBuilder.Entity<Proovedor>().ToTable("Proovedor");
        }
    }
}
