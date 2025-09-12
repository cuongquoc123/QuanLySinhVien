using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("LopHocPhan")]
public partial class LopHocPhan
{
    [Key]
    [Column("MaLopHP")]
    [StringLength(10)]
    public string MaLopHp { get; set; } = null!;

    [Column("TenLopHP")]
    [StringLength(100)]
    public string TenLopHp { get; set; } = null!;

    [StringLength(10)]
    public string MaMon { get; set; } = null!;

    [Column("MaGV")]
    [StringLength(10)]
    public string MaGv { get; set; } = null!;

    [StringLength(10)]
    public string? HocKy { get; set; }

    [StringLength(20)]
    public string? NamHoc { get; set; }

    public int SoTiet { get; set; }

    [InverseProperty("MaLopHpNavigation")]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();

    [InverseProperty("MaLopHpNavigation")]
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();

    [ForeignKey("MaGv")]
    [InverseProperty("LopHocPhans")]
    public virtual GiangVien MaGvNavigation { get; set; } = null!;

    [ForeignKey("MaMon")]
    [InverseProperty("LopHocPhans")]
    public virtual MonHoc MaMonNavigation { get; set; } = null!;

    [ForeignKey("MaLopHp")]
    [InverseProperty("MaLopHps")]
    public virtual ICollection<SinhVien> Mssvs { get; set; } = new List<SinhVien>();
}
