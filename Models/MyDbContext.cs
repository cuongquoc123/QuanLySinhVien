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

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<Cuahang> Cuahangs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Kho> Khos { get; set; }

    public virtual DbSet<LoaiDanhMuc> LoaiDanhMucs { get; set; }

    public virtual DbSet<Nguyenlieu> Nguyenlieus { get; set; }

    public virtual DbSet<PhieuNhapNl> PhieuNhapNls { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Sysrole> Sysroles { get; set; }

    public virtual DbSet<Sysuser> Sysusers { get; set; }

    public virtual DbSet<TonKho> TonKhos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.MaDon, e.MaSp }).HasName("PK__chi_tiet__EFFBA5E9DA84BEC5");

            entity.ToTable("chi_tiet_don_hang", tb => tb.HasTrigger("NganThemMoiSPChoDonXuLy"));

            entity.Property(e => e.MaDon).IsFixedLength();
            entity.Property(e => e.MaSp).IsFixedLength();

            entity.HasOne(d => d.MaDonNavigation).WithMany(p => p.ChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDH_DH");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDH_SP");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.MaNguyenLieu, e.MaPhieu }).HasName("PK__chi_tiet__D53798ABA9EE335A");

            entity.ToTable("chi_tiet_phieu_nhap", tb => tb.HasTrigger("UpdateSoLuongTonKho"));

            entity.Property(e => e.MaNguyenLieu).IsFixedLength();
            entity.Property(e => e.MaPhieu).IsFixedLength();

            entity.HasOne(d => d.MaNguyenLieuNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTYC_NL");

            entity.HasOne(d => d.MaPhieuNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTYC_PHIEU");
        });

        modelBuilder.Entity<Cuahang>(entity =>
        {
            entity.HasKey(e => e.CuaHangId).HasName("PK__cuahang__1BECA8F8F0AB2713");

            entity.Property(e => e.CuaHangId).IsFixedLength();
            entity.Property(e => e.Sdt).IsFixedLength();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D80EA633C5");

            entity.Property(e => e.CustomerId).IsFixedLength();
        });

        modelBuilder.Entity<CustomerDetail>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8553D5272");

            entity.Property(e => e.CustomerId).IsFixedLength();
            entity.Property(e => e.Avatar).HasDefaultValue("https://bla.edu.vn/wp-content/uploads/2025/09/avatar-fb.jpg");
            entity.Property(e => e.Cccd).IsFixedLength();
            entity.Property(e => e.Sdt).IsFixedLength();

            entity.HasOne(d => d.Customer).WithOne(p => p.CustomerDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detail_Customer");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.MaDm).HasName("PK__danhmuc__7A3EF40871B0208C");

            entity.Property(e => e.MaDm).IsFixedLength();
            entity.Property(e => e.MaLoaiDm).IsFixedLength();

            entity.HasOne(d => d.MaLoaiDmNavigation).WithMany(p => p.Danhmucs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DM_LDM");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.MaDon).HasName("PK__donhang__3D89F568E17DC050");

            entity.ToTable("donhang", tb => tb.HasTrigger("CapNhatGioHoanThanh"));

            entity.Property(e => e.MaDon).IsFixedLength();
            entity.Property(e => e.CustomerId).IsFixedLength();
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.Customer).WithMany(p => p.Donhangs).HasConstraintName("FK_Cus_DH");

            entity.HasOne(d => d.User).WithMany(p => p.Donhangs).HasConstraintName("FK_NV_DH");
        });

        modelBuilder.Entity<Kho>(entity =>
        {
            entity.HasKey(e => e.MaKho).HasName("PK__kho__3BDA9350EF0248CB");

            entity.Property(e => e.MaKho).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.Khos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kho_CH");
        });

        modelBuilder.Entity<LoaiDanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaLoaiDm).HasName("PK__LoaiDanh__1227485E62C8AE61");

            entity.Property(e => e.MaLoaiDm).IsFixedLength();
        });

        modelBuilder.Entity<Nguyenlieu>(entity =>
        {
            entity.HasKey(e => e.MaNguyenLieu).HasName("PK__nguyenli__C7519355E3741831");

            entity.Property(e => e.MaNguyenLieu).IsFixedLength();
        });

        modelBuilder.Entity<PhieuNhapNl>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__phieu_nh__2660BFE00267D449");

            entity.ToTable("phieu_nhapNL", tb => tb.HasTrigger("TuDienCotNgayNhap"));

            entity.Property(e => e.MaPhieu).IsFixedLength();
            entity.Property(e => e.MaKho).IsFixedLength();

            entity.HasOne(d => d.MaKhoNavigation).WithMany(p => p.PhieuNhapNls).HasConstraintName("FK_CH_PNL");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__sanpham__2725081CCF655094");

            entity.Property(e => e.MaSp).IsFixedLength();
            entity.Property(e => e.MaDm).IsFixedLength();

            entity.HasOne(d => d.MaDmNavigation).WithMany(p => p.Sanphams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SP_DM");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__staff__96D4AB177DD307CD");

            entity.ToTable("staff", tb =>
                {
                    tb.HasTrigger("KiemTraTuoiStaff");
                    tb.HasTrigger("NganChanXoaStaff");
                });

            entity.Property(e => e.StaffId).IsFixedLength();
            entity.Property(e => e.Avatar).HasDefaultValue("https://bla.edu.vn/wp-content/uploads/2025/09/avatar-fb.jpg");
            entity.Property(e => e.Cccd).IsFixedLength();
            entity.Property(e => e.CuaHangId).IsFixedLength();

            entity.HasOne(d => d.CuaHang).WithMany(p => p.Staff).HasConstraintName("FK_CH_STAFF");
        });

        modelBuilder.Entity<Sysrole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__sysrole__8AFACE1A5894A5A2");

            entity.Property(e => e.RoleId).IsFixedLength();
        });

        modelBuilder.Entity<Sysuser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__sysuser__1788CC4CBE03C44F");

            entity.Property(e => e.UserId).IsFixedLength();
            entity.Property(e => e.RoleId).IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Sysusers).HasConstraintName("FK_USER_ROLE");

            entity.HasOne(d => d.User).WithOne(p => p.Sysuser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_STAFF");
        });

        modelBuilder.Entity<TonKho>(entity =>
        {
            entity.Property(e => e.MaKho).IsFixedLength();
            entity.Property(e => e.MaNguyenLieu).IsFixedLength();

            entity.HasOne(d => d.MaKhoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kho_TonKho");

            entity.HasOne(d => d.MaNguyenLieuNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kho_NguyenLieu");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
