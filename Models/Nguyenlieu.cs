using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("nguyenlieu")]
public partial class Nguyenlieu
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNguyenLieu { get; set; } = null!;

    [StringLength(50)]
    public string TenNguyenLieu { get; set; } = null!;

    [Column("DVT")]
    [StringLength(20)]
    public string Dvt { get; set; } = null!;

    [InverseProperty("MaNguyenLieuNavigation")]
    public virtual ICollection<ChiTietYeuCau> ChiTietYeuCaus { get; set; } = new List<ChiTietYeuCau>();

    [InverseProperty("MaNguyenLieuNavigation")]
    public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();
}
