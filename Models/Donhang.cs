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

    [StringLength(50)]
    public string? TrangThai { get; set; }

    public DateTime NgayNhan { get; set; }

    public DateTime? NgayHoangThanh { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? CustomerId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [InverseProperty("MaDonNavigation")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [ForeignKey("CustomerId")]
    [InverseProperty("Donhangs")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Donhangs")]
    public virtual Sysuser? User { get; set; }
}
