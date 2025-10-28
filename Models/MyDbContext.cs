using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<Grn> Grns { get; set; }

    public virtual DbSet<Grndetail> Grndetails { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Sysrole> Sysroles { get; set; }

    public virtual DbSet<Sysuser> Sysusers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__category__19093A0B73BE255A");

            entity.Property(e => e.CategoryId).IsFixedLength();
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.GoodId).HasName("PK__goods__043AE53D8B3BA8E8");

            entity.Property(e => e.GoodId).IsFixedLength();
        });

        modelBuilder.Entity<Grn>(entity =>
        {
            entity.HasKey(e => e.GrnId).HasName("PK__GRN__ACCC6B89F6170161");

            entity.Property(e => e.GrnId).IsFixedLength();
            entity.Property(e => e.InventoryId).IsFixedLength();

            entity.HasOne(d => d.Inventory).WithMany(p => p.Grns).HasConstraintName("FK_CH_PNL");
        });

        modelBuilder.Entity<Grndetail>(entity =>
        {
            entity.HasKey(e => new { e.GoodId, e.GrnId }).HasName("PK__GRNDetai__8EF62385284A8FE1");

            entity.ToTable("GRNDetail", "management", tb => tb.HasTrigger("trg_update_Stock"));

            entity.Property(e => e.GoodId).IsFixedLength();
            entity.Property(e => e.GrnId).IsFixedLength();

            entity.HasOne(d => d.Good).WithMany(p => p.Grndetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GRN_Goods");

            entity.HasOne(d => d.Grn).WithMany(p => p.Grndetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detail_GRN");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__inventor__F5FDE6B3420C330E");

            entity.ToTable("inventory", "management", tb => tb.HasTrigger("trg_cancer_delete_inventory"));

            entity.Property(e => e.InventoryId).IsFixedLength();
            entity.Property(e => e.StoreId).IsFixedLength();

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Store");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__C3905BCFE133ACE8");

            entity.ToTable("orders", tb =>
                {
                    tb.HasTrigger("cancer_delete_Order");
                    tb.HasTrigger("trg_prevent_order_update");
                    tb.HasTrigger("trg_update_CompleteTime_Order");
                });

            entity.Property(e => e.OrderId).IsFixedLength();
            entity.Property(e => e.CustomerId).IsFixedLength();

            entity.HasOne(d => d.SysUser).WithMany(p => p.Orders).HasConstraintName("FK_SysUser_Order");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__order_de__08D097A35BCD0062");

            entity.ToTable("order_detail", tb => tb.HasTrigger("trg_lock_action_order_detail"));

            entity.Property(e => e.OrderId).IsFixedLength();
            entity.Property(e => e.ProductId).IsFixedLength();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__B40CC6CD73E287B0");

            entity.ToTable("products", tb => tb.HasTrigger("trg_cancer_delete_products"));

            entity.Property(e => e.ProductId).IsFixedLength();
            entity.Property(e => e.SubcategoryId).IsFixedLength();

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_SubCategory");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__staff__96D4AB174122ABC2");

            entity.ToTable("staff", "management", tb => tb.HasTrigger("CheckStaffAge"));

            entity.Property(e => e.StaffId).IsFixedLength();
            entity.Property(e => e.IdNumber).IsFixedLength();
            entity.Property(e => e.RoleId).IsFixedLength();
            entity.Property(e => e.StoreId).IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE");

            entity.HasOne(d => d.Store).WithMany(p => p.Staff).HasConstraintName("FK_Store_Staff");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.Property(e => e.GoodId).IsFixedLength();
            entity.Property(e => e.InventoryId).IsFixedLength();

            entity.HasOne(d => d.Good).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Goods");

            entity.HasOne(d => d.Inventory).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Stock");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__store__3B82F0E1F0F4934C");

            entity.ToTable("store", tb =>
                {
                    tb.HasTrigger("TrgCreateNewStore");
                    tb.HasTrigger("trg_cancer_delete_store");
                });

            entity.Property(e => e.StoreId).IsFixedLength();
            entity.Property(e => e.PhoneNum).IsFixedLength();
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__sub_cate__26BE5B19650684DB");

            entity.Property(e => e.SubCategoryId).IsFixedLength();
            entity.Property(e => e.CategoryId).IsFixedLength();

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_Category");
        });

        modelBuilder.Entity<Sysrole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__sysrole__8AFACE1A7BAFFD69");

            entity.Property(e => e.RoleId).IsFixedLength();
        });

        modelBuilder.Entity<Sysuser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__sysuser__1788CC4C908CAB42");

            entity.Property(e => e.RoleId).IsFixedLength();
            entity.Property(e => e.StaffId).IsFixedLength();
            entity.Property(e => e.StoreId).IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Sysusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_RoleS");

            entity.HasOne(d => d.Staff).WithMany(p => p.Sysusers).HasConstraintName("FK_User_Staff");

            entity.HasOne(d => d.Store).WithMany(p => p.Sysusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Store_SysUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
