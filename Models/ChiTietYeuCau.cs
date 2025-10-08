using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[PrimaryKey("MaNguyenLieu", "MaPhieu")]
[Table("chi_tiet_yeu_cau")]
public partial class ChiTietYeuCau
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNguyenLieu { get; set; } = null!;

    public int? SoLuong { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaPhieu { get; set; } = null!;

    [ForeignKey("MaNguyenLieu")]
    [InverseProperty("ChiTietYeuCaus")]
    public virtual Nguyenlieu MaNguyenLieuNavigation { get; set; } = null!;

    [ForeignKey("MaPhieu")]
    [InverseProperty("ChiTietYeuCaus")]
    public virtual PhieuNl MaPhieuNavigation { get; set; } = null!;
}
