using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("staff")]
public partial class Staff
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string StaffId { get; set; } = null!;

    [Column("CCCD")]
    [StringLength(11)]
    [Unicode(false)]
    public string Cccd { get; set; } = null!;

    [StringLength(50)]
    public string Ten { get; set; } = null!;

    [StringLength(50)]
    public string DiaChi { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    [Column(TypeName = "money")]
    public decimal? Luong { get; set; }

    [Column(TypeName = "money")]
    public decimal? Thuong { get; set; }

    [StringLength(500)]
    public string Avatar { get; set; } = null!;

    [StringLength(100)]
    public string? StatuSf { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? CuaHangId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [ForeignKey("CuaHangId")]
    [InverseProperty("Staff")]
    public virtual Cuahang? CuaHang { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    [ForeignKey("RoleId")]
    [InverseProperty("Staff")]
    public virtual Sysrole? Role { get; set; }

    [InverseProperty("User")]
    public virtual Sysuser? Sysuser { get; set; }
}
