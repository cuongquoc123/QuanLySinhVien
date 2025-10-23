using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[PrimaryKey("MaNguyenLieu", "MaPhieu")]
[Table("chi_tiet_phieu_nhap")]
public partial class ChiTietPhieuNhap
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNguyenLieu { get; set; } = null!;

    public int SoLuong { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaPhieu { get; set; } = null!;

    [ForeignKey("MaNguyenLieu")]
    [InverseProperty("ChiTietPhieuNhaps")]
    public virtual Nguyenlieu MaNguyenLieuNavigation { get; set; } = null!;

    [ForeignKey("MaPhieu")]
    [InverseProperty("ChiTietPhieuNhaps")]
    public virtual PhieuNhapNl MaPhieuNavigation { get; set; } = null!;
}
