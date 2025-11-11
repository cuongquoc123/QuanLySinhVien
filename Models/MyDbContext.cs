using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.SqlDTO;

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

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Inventoryrecord> Inventoryrecords { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RecorDetail> RecorDetails { get; set; }

    public virtual DbSet<Recordtype> Recordtypes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Sysrole> Sysroles { get; set; }

    public virtual DbSet<Sysuser> Sysusers { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoreAccountResult>().HasNoKey();
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__category__19093A0BEAD1368A");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CategoryName).HasMaxLength(10);
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.GoodId).HasName("PK__goods__043AE53DA1A546C5");

            entity.ToTable("goods", "management");

            entity.Property(e => e.GoodId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GoodName).HasMaxLength(100);
            entity.Property(e => e.UnitName).HasMaxLength(20);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__inventor__F5FDE6B31B9C2F78");

            entity.ToTable("inventory", "management", tb => tb.HasTrigger("trg_cancer_delete_inventory"));

            entity.Property(e => e.Addr).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.StoreId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("StoreID");

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Store");
        });

        modelBuilder.Entity<Inventoryrecord>(entity =>
        {
            entity.HasKey(e => e.RecordsId).HasName("PK__inventor__4C9899878B1F4649");

            entity.ToTable("inventoryrecord", "management");

            entity.Property(e => e.RecordsId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Inventory).WithMany(p => p.Inventoryrecords)
                .HasForeignKey(d => d.InventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CH_PNL");

            entity.HasOne(d => d.Type).WithMany(p => p.Inventoryrecords)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReCord_Type");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__C3905BCFAA5CB8D6");

            entity.ToTable("orders", tb =>
                {
                    tb.HasTrigger("cancer_delete_Order");
                    tb.HasTrigger("trg_prevent_order_update");
                    tb.HasTrigger("trg_update_CompleteTime_Order");
                });

            entity.Property(e => e.OrderId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CustomerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.SysUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SysUserId)
                .HasConstraintName("FK_SysUser_Order");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__order_de__08D097A3E8AA0DE9");

            entity.ToTable("order_detail", tb => tb.HasTrigger("trg_lock_action_order_detail"));

            entity.Property(e => e.OrderId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__B40CC6CDA45CAB22");

            entity.ToTable("products", tb => tb.HasTrigger("trg_cancer_delete_products"));

            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Decription).HasColumnType("text");
            entity.Property(e => e.Img)
                .HasMaxLength(500)
                .HasColumnName("IMG");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.SubcategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubcategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_SubCategory");
        });

        modelBuilder.Entity<RecorDetail>(entity =>
        {
            entity.HasKey(e => new { e.GoodId, e.RecordsId }).HasName("PK__RecorDet__60F36CA5C128CB87");

            entity.ToTable("RecorDetail", "management");

            entity.Property(e => e.GoodId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RecordsId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Good).WithMany(p => p.RecorDetails)
                .HasForeignKey(d => d.GoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Record_Goods");

            entity.HasOne(d => d.Records).WithMany(p => p.RecorDetails)
                .HasForeignKey(d => d.RecordsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detail_Record");
        });

        modelBuilder.Entity<Recordtype>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__recordty__516F03B5F13A5A3B");

            entity.ToTable("recordtype", "management");

            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__staff__96D4AB171617788A");

            entity.ToTable("staff", "management", tb => tb.HasTrigger("CheckStaffAge"));

            entity.Property(e => e.StaffId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.Bonus).HasColumnType("money");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(5);
            entity.Property(e => e.IdNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RoleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Salary).HasColumnType("money");
            entity.Property(e => e.StaffAddr).HasMaxLength(50);
            entity.Property(e => e.StaffName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.StoreId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE");

            entity.HasOne(d => d.Store).WithMany(p => p.Staff)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Store_Staff");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.InventoryId, e.GoodId });

            entity.ToTable("Stock", "management");

            entity.Property(e => e.GoodId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.Good).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.GoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Goods");

            entity.HasOne(d => d.Inventory).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.InventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Stock");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__store__3B82F0E1431E9F43");

            entity.ToTable("store", tb =>
                {
                    tb.HasTrigger("TrgCreateNewStore");
                    tb.HasTrigger("trg_cancer_delete_store");
                });

            entity.Property(e => e.StoreId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("StoreID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StoreAddr).HasMaxLength(100);
            entity.Property(e => e.StoreName).HasMaxLength(100);
            entity.Property(e => e.StoreStatus)
                .HasMaxLength(100)
                .HasColumnName("Store_Status");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__sub_cate__26BE5B19AE0F1AE7");

            entity.ToTable("sub_category");

            entity.Property(e => e.SubCategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SubCategoryName).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_Category");
        });

        modelBuilder.Entity<Sysrole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__sysrole__8AFACE1AC021173F");

            entity.ToTable("sysrole", "management");

            entity.Property(e => e.RoleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Sysuser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__sysuser__1788CC4C77FE6BFD");

            entity.ToTable("sysuser", "management");

            entity.HasIndex(e => e.UserName, "UQ__sysuser__C9F284565BE08D09").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RoleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StaffId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StoreId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Sysusers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_RoleS");

            entity.HasOne(d => d.Staff).WithMany(p => p.Sysusers)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_User_Staff");

            entity.HasOne(d => d.Store).WithMany(p => p.Sysusers)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Store_SysUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
