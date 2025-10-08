using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("cuahang")]
public partial class Cuahang
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string CuaHangId { get; set; } = null!;

    [Column("TenCH")]
    [StringLength(50)]
    public string TenCh { get; set; } = null!;

    [StringLength(50)]
    public string DiaChi { get; set; } = null!;

    [Column("SDT")]
    [StringLength(11)]
    [Unicode(false)]
    public string? Sdt { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [InverseProperty("CuaHang")]
    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    [InverseProperty("CuaHang")]
    public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();

    [InverseProperty("CuaHang")]
    public virtual ICollection<PhieuNl> PhieuNls { get; set; } = new List<PhieuNl>();

    [InverseProperty("CuaHang")]
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    [InverseProperty("CuaHang")]
    public virtual ICollection<Sysuser> Sysusers { get; set; } = new List<Sysuser>();
}
