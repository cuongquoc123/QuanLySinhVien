using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("phieu_nhapNL")]
public partial class PhieuNhapNl
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaPhieu { get; set; } = null!;

    [Column("ngay_nhap")]
    public DateTime NgayNhap { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKho { get; set; }

    [InverseProperty("MaPhieuNavigation")]
    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    [ForeignKey("MaKho")]
    [InverseProperty("PhieuNhapNls")]
    public virtual Kho? MaKhoNavigation { get; set; }
}
