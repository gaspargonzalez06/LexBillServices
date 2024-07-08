
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace LexBillServicesApp.Models
{
    public class LexBillDataContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<TipoCambio> TiposCambio { get; set; }

        public LexBillDataContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["LexBill"].ConnectionString;
        }

        public LexBillDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DetallePedido>()
        //        .HasKey(dp => dp.DetallePedidoID);

        //    modelBuilder.Entity<Factura>()
        //        .HasKey(f => f.FacturaID);

        //    modelBuilder.Entity<Pedido>()
        //        .HasKey(p => p.PedidoID);

        //    modelBuilder.Entity<Producto>()
        //        .HasKey(p => p.ProductoID);

        //    modelBuilder.Entity<Categoria>()
        //        .HasKey(c => c.CategoriaID);

        //    modelBuilder.Entity<Cliente>()
        //        .HasKey(c => c.ClienteID);

        //    modelBuilder.Entity<TipoCambio>()
        //        .HasKey(tc => tc.TipoCambioID);

        //    // Relaciones
        //    modelBuilder.Entity<Pedido>()
        //        .HasMany(p => p.DetallePedidos)
        //        .WithOne(dp => dp.Pedido)
        //        .HasForeignKey(dp => dp.PedidoID)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<DetallePedido>()
        //        .HasOne(dp => dp.Producto)
        //        .WithMany(p => p.DetallePedidos)
        //        .HasForeignKey(dp => dp.ProductoID);

        //    modelBuilder.Entity<Factura>()
        //        .HasOne(f => f.Pedido)
        //        .WithMany(p => p.Facturas)
        //        .HasForeignKey(f => f.PedidoID);

        //    modelBuilder.Entity<TipoCambio>()
        //        .HasMany(tc => tc.Facturas)
        //        .WithOne(f => f.TipoCambio)
        //        .HasForeignKey(f => f.TipoCambioID)
        //        .OnDelete(DeleteBehavior.Cascade);
        //}
    }
}