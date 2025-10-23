using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("kho")]
public partial class Kho
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaKho { get; set; } = null!;

    [StringLength(50)]
    public string? TrangThai { get; set; }

    [StringLength(50)]
    public string? DiaChi { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string CuaHangId { get; set; } = null!;

    [ForeignKey("CuaHangId")]
    [InverseProperty("Khos")]
    public virtual Cuahang CuaHang { get; set; } = null!;

    [InverseProperty("MaKhoNavigation")]
    public virtual ICollection<PhieuNhapNl> PhieuNhapNls { get; set; } = new List<PhieuNhapNl>();
}
