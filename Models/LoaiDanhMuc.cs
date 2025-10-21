using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("LoaiDanhMuc")]
public partial class LoaiDanhMuc
{
    [Key]
    [Column("MaLoaiDM")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaLoaiDm { get; set; } = null!;

    [Column("TenLoaiDM")]
    [StringLength(10)]
    public string? TenLoaiDm { get; set; }

    [InverseProperty("MaLoaiDmNavigation")]
    public virtual ICollection<Danhmuc> Danhmucs { get; set; } = new List<Danhmuc>();
}
