using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("danhmuc")]
public partial class Danhmuc
{
    [Key]
    [Column("maDM")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDm { get; set; } = null!;

    [Column("tenDM")]
    [StringLength(50)]
    public string TenDm { get; set; } = null!;

    [Column("MaLoaiDM")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaLoaiDm { get; set; } = null!;

    [ForeignKey("MaLoaiDm")]
    [InverseProperty("Danhmucs")]
    public virtual LoaiDanhMuc MaLoaiDmNavigation { get; set; } = null!;

    [InverseProperty("MaDmNavigation")]
    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
