using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[PrimaryKey("MaDon", "MaSp")]
[Table("chi_tiet_don_hang")]
public partial class ChiTietDonHang
{
    public int SoLuong { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDon { get; set; } = null!;

    [Key]
    [Column("MaSP")]
    [StringLength(20)]
    [Unicode(false)]
    public string MaSp { get; set; } = null!;

    [ForeignKey("MaDon")]
    [InverseProperty("ChiTietDonHangs")]
    public virtual Donhang MaDonNavigation { get; set; } = null!;

    [ForeignKey("MaSp")]
    [InverseProperty("ChiTietDonHangs")]
    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
