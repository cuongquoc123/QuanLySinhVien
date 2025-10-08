using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("donhang")]
public partial class Donhang
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDon { get; set; } = null!;

    [Column(TypeName = "decimal(4, 2)")]
    public decimal ThanhTien { get; set; }

    [StringLength(50)]
    public string? TrangThai { get; set; }

    public DateOnly NgayNhan { get; set; }

    public DateOnly? NgayHoangThanh { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string CuaHangId { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    [InverseProperty("MaDonNavigation")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [ForeignKey("CuaHangId")]
    [InverseProperty("Donhangs")]
    public virtual Cuahang CuaHang { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Donhangs")]
    public virtual Sysuser User { get; set; } = null!;
}
