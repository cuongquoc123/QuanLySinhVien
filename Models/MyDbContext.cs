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
            entity.HasKey(e => e.MaDiemDanh).HasName("PK__DiemDanh__1512439D452631EC");

            entity.Property(e => e.MaLopHp).IsFixedLength();
            entity.Property(e => e.Mssv).IsFixedLength();

            entity.HasOne(d => d.MaLichHocNavigation).WithMany(p => p.DiemDanhs).HasConstraintName("FK__DiemDanh__MaLich__5AEE82B9");

            entity.HasOne(d => d.MaLopHpNavigation).WithMany(p => p.DiemDanhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiemDanh__MaLopH__571DF1D5");

            entity.HasOne(d => d.MssvNavigation).WithMany(p => p.DiemDanhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiemDanh__MSSV__5629CD9C");
        });

        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("PK__GiangVie__2725AEF3B50D7B8A");

            entity.Property(e => e.MaGv).IsFixedLength();
            entity.Property(e => e.MaKhoa).IsFixedLength();

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.GiangViens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiangVien__MaKho__440B1D61");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.MaKhoa).HasName("PK__Khoa__6539040544DD51CC");

            entity.Property(e => e.MaKhoa).IsFixedLength();
        });

        modelBuilder.Entity<LichHoc>(entity =>
        {
            entity.HasKey(e => e.MaLichHoc).HasName("PK__LichHoc__150EBC213CAC7493");

            entity.Property(e => e.MaLopHp).IsFixedLength();

            entity.HasOne(d => d.MaLopHpNavigation).WithMany(p => p.LichHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichHoc__MaLopHP__59FA5E80");
        });

        modelBuilder.Entity<LopHanhChinh>(entity =>
        {
            entity.HasKey(e => e.MaLopHc).HasName("PK__LopHanhC__976ACA0F42EE3B50");

            entity.Property(e => e.MaLopHc).IsFixedLength();
            entity.Property(e => e.MaNganh).IsFixedLength();

            entity.HasOne(d => d.MaNganhNavigation).WithMany(p => p.LopHanhChinhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LopHanhCh__MaNga__3B75D760");
        });

        modelBuilder.Entity<LopHocPhan>(entity =>
        {
            entity.HasKey(e => e.MaLopHp).HasName("PK__LopHocPh__976ACA3229293036");

            entity.Property(e => e.MaLopHp).IsFixedLength();
            entity.Property(e => e.MaGv).IsFixedLength();
            entity.Property(e => e.MaMon).IsFixedLength();

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.LopHocPhans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GV_LHP");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.LopHocPhans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MH_LHP");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMon).HasName("PK__MonHoc__3A5B29A871EE7D5D");

            entity.Property(e => e.MaMon).IsFixedLength();
            entity.Property(e => e.MaNganh).IsFixedLength();

            entity.HasOne(d => d.MaNganhNavigation).WithMany(p => p.MonHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MonHoc__MaNganh__46E78A0C");
        });

        modelBuilder.Entity<Nganh>(entity =>
        {
            entity.HasKey(e => e.MaNganh).HasName("PK__Nganh__A2CEF50DD986D40C");

            entity.Property(e => e.MaNganh).IsFixedLength();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__CD98462A5111BB9C");

            entity.Property(e => e.RoleId).IsFixedLength();
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.Mssv).HasName("PK__SinhVien__6CB3B7F9E545C0D1");

            entity.Property(e => e.Mssv).IsFixedLength();
            entity.Property(e => e.MaLopHc).IsFixedLength();

            entity.HasOne(d => d.MaLopHcNavigation).WithMany(p => p.SinhViens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SinhVien__MaLopH__412EB0B6");

            entity.HasMany(d => d.MaLopHps).WithMany(p => p.Mssvs)
                .UsingEntity<Dictionary<string, object>>(
                    "DangKyLopHp",
                    r => r.HasOne<LopHocPhan>().WithMany()
                        .HasForeignKey("MaLopHp")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DangKyLop__MaLop__4E88ABD4"),
                    l => l.HasOne<SinhVien>().WithMany()
                        .HasForeignKey("Mssv")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DangKyLopH__MSSV__4D94879B"),
                    j =>
                    {
                        j.HasKey("Mssv", "MaLopHp").HasName("PK__DangKyLo__55C51B5A9E3BD083");
                        j.ToTable("DangKyLopHP");
                        j.IndexerProperty<string>("Mssv")
                            .HasMaxLength(15)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("MSSV");
                        j.IndexerProperty<string>("MaLopHp")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("MaLopHP");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__users__F3DBC5737CECC4F8");

            entity.Property(e => e.Username).IsFixedLength();
            entity.Property(e => e.RoleId).IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("FK_USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
