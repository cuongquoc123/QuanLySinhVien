using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("DiemDanh")]
public partial class DiemDanh
{
    [Key]
    public int MaDiemDanh { get; set; }

    [Column("MSSV")]
    [StringLength(15)]
    [Unicode(false)]
    public string Mssv { get; set; } = null!;

    [Column("MaLopHP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaLopHp { get; set; } = null!;

    public DateOnly NgayHoc { get; set; }

    [StringLength(20)]
    public string? TrangThai { get; set; }

    public int? MaLichHoc { get; set; }

    [ForeignKey("MaLichHoc")]
    [InverseProperty("DiemDanhs")]
    public virtual LichHoc? MaLichHocNavigation { get; set; }

    [ForeignKey("MaLopHp")]
    [InverseProperty("DiemDanhs")]
    public virtual LopHocPhan MaLopHpNavigation { get; set; } = null!;

    [ForeignKey("Mssv")]
    [InverseProperty("DiemDanhs")]
    public virtual SinhVien MssvNavigation { get; set; } = null!;
}
