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

    public int? SoLuongTon { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaNguyenLieu { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? CuaHangId { get; set; }

    [ForeignKey("CuaHangId")]
    [InverseProperty("Khos")]
    public virtual Cuahang? CuaHang { get; set; }

    [ForeignKey("MaNguyenLieu")]
    [InverseProperty("Khos")]
    public virtual Nguyenlieu? MaNguyenLieuNavigation { get; set; }
}
