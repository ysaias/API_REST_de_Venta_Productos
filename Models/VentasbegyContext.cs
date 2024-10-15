using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace begywebsapi.Models;

public partial class VentasbegyContext : IdentityDbContext
{
   
    public VentasbegyContext(DbContextOptions<VentasbegyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoItem> PedidoItems { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usaurios { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Usaurios");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CartItems_Carts");

            entity.HasOne(d => d.Producto).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CartItems_Productos");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.Property(e => e.FechaDePago).HasColumnType("datetime");
            entity.Property(e => e.MetodoDePago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MontoDePago).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pagos_Pedidos");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Usaurios");
        });

        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoItems)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoItems_Pedidos");

            entity.HasOne(d => d.Producto).WithMany(p => p.PedidoItems)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoItems_Productos");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK_Productos_Categorias");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleNombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Usaurios)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usaurios_Roles");
        });

       base.OnModelCreating(modelBuilder);
    }

    
}
