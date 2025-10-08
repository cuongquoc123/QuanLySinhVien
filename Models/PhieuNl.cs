using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("phieu_NL")]
public partial class PhieuNl
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaPhieu { get; set; } = null!;

    [Column("ngay_yeu_cau")]
    public DateOnly NgayYeuCau { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? CuaHangId { get; set; }

    [InverseProperty("MaPhieuNavigation")]
    public virtual ICollection<ChiTietYeuCau> ChiTietYeuCaus { get; set; } = new List<ChiTietYeuCau>();

    [ForeignKey("CuaHangId")]
    [InverseProperty("PhieuNls")]
    public virtual Cuahang? CuaHang { get; set; }
}
