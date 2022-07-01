using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemPedido>()
                .HasKey(e => new { e.PedidoId, e.ProdutoId });

            //restringe a exclusão de clientes que possuem pedidos
            modelBuilder.Entity<Pedido>()
                .HasOne<Cliente>(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            //exclui automaticamente os itens de um pedido quando um pedido é excluído
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Pedido>(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(p => p.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            //restringe exclusão de produtos que possuem itens pedidos
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Produto>(ip => ip.Produto)
                .WithMany()
                .HasForeignKey(p => p.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ItemPedido> ItemPedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

    }
}
