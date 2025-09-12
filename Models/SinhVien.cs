using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("SinhVien")]
public partial class SinhVien
{
    [Key]
    [Column("MSSV")]
    [StringLength(15)]
    public string Mssv { get; set; } = null!;

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [Column("SDT")]
    [StringLength(20)]
    public string? Sdt { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? DiaChi { get; set; }

    [Column("MaLopHC")]
    [StringLength(10)]
    public string MaLopHc { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Avatar { get; set; }

    [InverseProperty("MssvNavigation")]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();

    [ForeignKey("MaLopHc")]
    [InverseProperty("SinhViens")]
    public virtual LopHanhChinh MaLopHcNavigation { get; set; } = null!;

    [ForeignKey("Mssv")]
    [InverseProperty("Mssvs")]
    public virtual ICollection<LopHocPhan> MaLopHps { get; set; } = new List<LopHocPhan>();
}
