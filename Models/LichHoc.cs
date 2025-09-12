using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("LichHoc")]
public partial class LichHoc
{
    [Key]
    public int MaLichHoc { get; set; }

    [Column("MaLopHP")]
    [StringLength(10)]
    public string MaLopHp { get; set; } = null!;

    public DateOnly NgayHoc { get; set; }

    public int TietBatDau { get; set; }

    public int SoTiet { get; set; }

    [StringLength(50)]
    public string? PhongHoc { get; set; }

    [StringLength(200)]
    public string? GhiChu { get; set; }

    [StringLength(10)]
    public string DayOfWeek { get; set; } = null!;

    [InverseProperty("MaLichHocNavigation")]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();

    [ForeignKey("MaLopHp")]
    [InverseProperty("LichHocs")]
    public virtual LopHocPhan MaLopHpNavigation { get; set; } = null!;
}
