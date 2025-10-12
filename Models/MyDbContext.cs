using System;
using System.Collections.Generic;
using DotNetEnv;
using Microsoft.Data.SqlClient;
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

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietYeuCau> ChiTietYeuCaus { get; set; }

    public virtual DbSet<Cuahang> Cuahangs { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Kho> Khos { get; set; }

    public virtual DbSet<Nguyenlieu> Nguyenlieus { get; set; }

    public virtual DbSet<PhieuNl> PhieuNls { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Sysrole> Sysroles { get; set; }

    public virtual DbSet<Sysuser> Sysusers { get; set; }

    static string? Server = Env.GetString("db_host2") ; 
    static string? database = Env.GetString("db2");
    static string User = Env.GetString("db_user2");
    static string Password = Env.GetString("db_password2");
    static string TrustServerCertificate = Env.GetString("TrustServerCertificate");
    static string ConnStr = $"Server={Server};Database={database};User Id={User};Password={Password};TrustServerCertificate={TrustServerCertificate};";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConnStr);

    SqlConnection con = new SqlConnection(ConnStr);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.MaDon, e.MaSp }).HasName("PK__chi_tiet__EFFBA5E9AEBA59AB");

            entity.Property(e => e.MaDon).IsFixedLength();
            entity.Property(e => e.MaSp).IsFixedLength();

            entity.HasOne(d => d.MaDonNavigation).WithMany(p => p.ChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDH_DH");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDH_SP");
        });

        modelBuilder.Entity<ChiTietYeuCau>(entity =>
        {
            entity.HasKey(e => new { e.MaNguyenLieu, e.MaPhieu }).HasName("PK__chi_tiet__D53798ABAED2B5B4");

            entity.Property(e => e.MaNguyenLieu).IsFixedLength();
            entity.Property(e => e.MaPhieu).IsFixedLength();

            entity.HasOne(d => d.MaNguyenLieuNavigation).WithMany(p => p.ChiTietYeuCaus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTYC_NL");

            entity.HasOne(d => d.MaPhieuNavigation).WithMany(p => p.ChiTietYeuCaus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTYC_PHIEU");
        });

        modelBuilder.Entity<Cuahang>(entity =>
        {
            entity.HasKey(e => e.CuaHangId).HasName("PK__cuahang__1BECA8F8C0C32786");

            entity.Property(e => e.CuaHangId).IsFixedLength();
            entity.Property(e => e.Sdt).IsFixedLength();
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.MaDm).HasName("PK__danhmuc__7A3EF408A47B3096");

            entity.Property(e => e.MaDm).IsFixedLength();
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.MaDon).HasName("PK__donhang__3D89F568DADE1B50");

            entity.Property(e => e.MaDon).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.Donhangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CH_DH");

            entity.HasOne(d => d.User).WithMany(p => p.Donhangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NV_DH");
        });

        modelBuilder.Entity<Kho>(entity =>
        {
            entity.HasKey(e => e.MaKho).HasName("PK__kho__3BDA9350B6F69AB0");

            entity.Property(e => e.MaKho).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();
            entity.Property(e => e.MaNguyenLieu).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.Khos).HasConstraintName("FK_Kho_CH");

            entity.HasOne(d => d.MaNguyenLieuNavigation).WithMany(p => p.Khos).HasConstraintName("FK_Kho_NguyenLieu");
        });

        modelBuilder.Entity<Nguyenlieu>(entity =>
        {
            entity.HasKey(e => e.MaNguyenLieu).HasName("PK__nguyenli__C7519355A690F357");

            entity.Property(e => e.MaNguyenLieu).IsFixedLength();
        });

        modelBuilder.Entity<PhieuNl>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__phieu_NL__2660BFE0512F08CE");

            entity.Property(e => e.MaPhieu).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.PhieuNls).HasConstraintName("FK_CH_PNL");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__sanpham__2725081C4A783281");

            entity.Property(e => e.MaSp).IsFixedLength();
            entity.Property(e => e.MaDm).IsFixedLength();

            entity.HasOne(d => d.MaDmNavigation).WithMany(p => p.Sanphams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SP_DM");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Cccd).HasName("PK__staff__A955A0AB1ECBB64F");

            entity.Property(e => e.Cccd).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.Staff).HasConstraintName("FK_CH_STAFF");
        });

        modelBuilder.Entity<Sysrole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__sysrole__8AFACE1A7623A76D");

            entity.Property(e => e.RoleId).IsFixedLength();
        });

        modelBuilder.Entity<Sysuser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__sysuser__1788CC4C7B96833B");

            entity.Property(e => e.UserId).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();
            entity.Property(e => e.RoleId).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.Sysusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CH_User");

            entity.HasOne(d => d.Role).WithMany(p => p.Sysusers).HasConstraintName("FK_USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public void TestConnect()
    {
        try
        {
            con.Open();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }
}
