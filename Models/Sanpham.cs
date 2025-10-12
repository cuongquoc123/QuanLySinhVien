using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sanpham")]
public partial class Sanpham
{
    [Key]
    [Column("MaSP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaSp { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DonGia { get; set; }

    [Column("TenSP")]
    [StringLength(50)]
    public string TenSp { get; set; } = null!;

    [StringLength(500)]
    public string? Anh { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "text")]
    public string? Mota { get; set; }

    [Column("maDM")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDm { get; set; } = null!;

    [InverseProperty("MaSpNavigation")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [ForeignKey("MaDm")]
    [InverseProperty("Sanphams")]
    public virtual Danhmuc MaDmNavigation { get; set; } = null!;
}
