using System;
using System.Collections.Generic;
using DotNetEnv;
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

    public virtual DbSet<DiemDanh> DiemDanhs { get; set; }

    public virtual DbSet<GiangVien> GiangViens { get; set; }

    public virtual DbSet<Khoa> Khoas { get; set; }

    public virtual DbSet<LichHoc> LichHocs { get; set; }

    public virtual DbSet<LopHanhChinh> LopHanhChinhs { get; set; }

    public virtual DbSet<LopHocPhan> LopHocPhans { get; set; }

    public virtual DbSet<MonHoc> MonHocs { get; set; }

    public virtual DbSet<Nganh> Nganhs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    static string db_host2 = Env.GetString("db_host2");
    static string db2 = Env.GetString("db2");
    static string db_user2 = Env.GetString("db_user2");
    static string db_password2 = Env.GetString("db_password2");
    static string TrustServerCertificate = Env.GetString("TrustServerCertificate");
    static string connectionString2 = $"Server={db_host2};Database={db2};User Id={db_user2};Password={db_password2};TrustServerCertificate={TrustServerCertificate};";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(connectionString2);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiemDanh>(entity =>
        {
            entity.HasKey(e => e.MaDiemDanh).HasName("PK__DiemDanh__1512439DAB814353");

            entity.HasOne(d => d.MaLichHocNavigation).WithMany(p => p.DiemDanhs).HasConstraintName("FK__DiemDanh__MaLich__5441852A");

            entity.HasOne(d => d.MaLopHpNavigation).WithMany(p => p.DiemDanhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiemDanh__MaLopH__5070F446");

            entity.HasOne(d => d.MssvNavigation).WithMany(p => p.DiemDanhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiemDanh__MSSV__4F7CD00D");
        });

        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("PK__GiangVie__2725AEF305148571");

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.GiangViens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiangVien__MaKho__412EB0B6");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.MaKhoa).HasName("PK__Khoa__65390405A9CFCAEA");
        });

        modelBuilder.Entity<LichHoc>(entity =>
        {
            entity.HasKey(e => e.MaLichHoc).HasName("PK__LichHoc__150EBC2162391E1B");

            entity.HasOne(d => d.MaLopHpNavigation).WithMany(p => p.LichHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichHoc__MaLopHP__534D60F1");
        });

        modelBuilder.Entity<LopHanhChinh>(entity =>
        {
            entity.HasKey(e => e.MaLopHc).HasName("PK__LopHanhC__976ACA0F63AA80EA");

            entity.HasOne(d => d.MaNganhNavigation).WithMany(p => p.LopHanhChinhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LopHanhCh__MaNga__3B75D760");
        });

        modelBuilder.Entity<LopHocPhan>(entity =>
        {
            entity.HasKey(e => e.MaLopHp).HasName("PK__LopHocPh__976ACA32A10F4CEC");

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.LopHocPhans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LopHocPhan__MaGV__47DBAE45");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.LopHocPhans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LopHocPha__MaMon__46E78A0C");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMon).HasName("PK__MonHoc__3A5B29A8594D618C");

            entity.HasOne(d => d.MaNganhNavigation).WithMany(p => p.MonHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MonHoc__MaNganh__440B1D61");
        });

        modelBuilder.Entity<Nganh>(entity =>
        {
            entity.HasKey(e => e.MaNganh).HasName("PK__Nganh__A2CEF50DD788395B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__CD98462A29D160AF");

            entity.Property(e => e.RoleId).IsFixedLength();
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.Mssv).HasName("PK__SinhVien__6CB3B7F94CD7C14E");

            entity.HasOne(d => d.MaLopHcNavigation).WithMany(p => p.SinhViens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SinhVien__MaLopH__3E52440B");

            entity.HasMany(d => d.MaLopHps).WithMany(p => p.Mssvs)
                .UsingEntity<Dictionary<string, object>>(
                    "DangKyLopHp",
                    r => r.HasOne<LopHocPhan>().WithMany()
                        .HasForeignKey("MaLopHp")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DangKyLop__MaLop__4BAC3F29"),
                    l => l.HasOne<SinhVien>().WithMany()
                        .HasForeignKey("Mssv")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DangKyLopH__MSSV__4AB81AF0"),
                    j =>
                    {
                        j.HasKey("Mssv", "MaLopHp").HasName("PK__DangKyLo__55C51B5AFC016154");
                        j.ToTable("DangKyLopHP");
                        j.IndexerProperty<string>("Mssv")
                            .HasMaxLength(15)
                            .HasColumnName("MSSV");
                        j.IndexerProperty<string>("MaLopHp")
                            .HasMaxLength(10)
                            .HasColumnName("MaLopHP");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__users__F3DBC57382CA53C7");

            entity.Property(e => e.Username).IsFixedLength();
            entity.Property(e => e.RoleId).IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("FK_USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
