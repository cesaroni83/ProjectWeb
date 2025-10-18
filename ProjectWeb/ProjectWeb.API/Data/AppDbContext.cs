using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Google;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Data
{
    public class AppDbContext : IdentityDbContext<User>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Pais> Tbl_Pais { get; set; }
        public DbSet<Provincia> Tbl_Provincia { get; set; }
        public DbSet<Ciudad> Tbl_Ciudad { get; set; }
        public DbSet<Empresa> Tbl_Empresa { get; set; }
        public DbSet<Sucursal> Tbl_Sucursal { get; set; }
        public DbSet<Persona> Tbl_Persona { get; set; }
        public DbSet<Menu> Tbl_Menu { get; set; }

        public DbSet<Credential> Credentials => Set<Credential>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //claves primarias
            ///indices 
            modelBuilder.Entity<Pais>().HasIndex(x => x.Nombre_pais).IsUnique(); ///Crea  un indece unico en el nombre
            modelBuilder.Entity<Provincia>().HasIndex(x => x.Nombre_provincia).IsUnique(); ///Crea  un indece unico en el nombre
            modelBuilder.Entity<Ciudad>().HasIndex(x => x.Nombre_ciudad).IsUnique(); ///Crea  un indece unico en el nombre
            modelBuilder.Entity<Empresa>().HasIndex(x => x.Nombre_Empresa).IsUnique(); ///Crea  un indece unico en el nombre
            modelBuilder.Entity<Sucursal>().HasIndex(x => x.Nombre_sucursal).IsUnique(); ///Crea  un indece unico en el nombre
            modelBuilder.Entity<Persona>().HasIndex(x => x.Numero_documento).IsUnique(); ///Crea  un indece unico en el nombre
            modelBuilder.Entity<Ciudad>().HasIndex(x => x.Nombre_ciudad).IsUnique(); ///Crea  un indece unico en el nombre
            //// relacionar de tablas
            modelBuilder.Entity<Provincia>().HasIndex(p => new { p.Id_pais, p.Nombre_provincia }).IsUnique();
            modelBuilder.Entity<Ciudad>().HasIndex(p => new { p.Id_provincia, p.Nombre_ciudad }).IsUnique();
            modelBuilder.Entity<Sucursal>().HasIndex(p => new { p.Id_empresa, p.Nombre_sucursal }).IsUnique();

            /// ******************
            // Relación Pais -> Provincia (uno a muchos)
            modelBuilder.Entity<Provincia>()
            .HasOne(p => p.Paises)
            .WithMany(pa => pa.Provincias)
            .HasForeignKey(p => p.Id_pais) // aquí le dices que Id_pais es la FK
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ciudad>()
           .HasOne(c => c.Provincias)
           .WithMany(p => p.Ciudades)
           .HasForeignKey(c => c.Id_provincia)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sucursal>()
           .HasOne(c => c.Empresas)
           .WithMany(p => p.Sucursales)
           .HasForeignKey(c => c.Id_empresa)
           .OnDelete(DeleteBehavior.Cascade);
            ///--------- usuarioas


            modelBuilder.Entity<User>()
            .HasOne(e => e.Ciudades)
            .WithMany(c => c.Users)
            .HasForeignKey(e => e.Id_ciudad)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Sucursal>()
            .HasOne(e => e.Ciudades)
            .WithMany(c => c.Sucursales)
            .HasForeignKey(e => e.Id_ciudad)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Empresa>()
             .HasOne(e => e.Ciudades)
             .WithMany(c => c.Empresas)
             .HasForeignKey(e => e.Id_ciudad)
             .OnDelete(DeleteBehavior.Restrict);

            // Ciudad ↔ Persona
            modelBuilder.Entity<Persona>()
                .HasOne(p => p.Ciudades)
                .WithMany(c => c.Personas)
                .HasForeignKey(p => p.Id_ciudad)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Persona>()
            //    .HasOne(p => p.Users)
            //    .WithOne(u => u.Personas)
            //    .HasForeignKey<User>(u => u.Id_persona) // FK en User
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
