using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sysuser")]
[Index("UserName", Name = "UQ__sysuser__C9F284560D44C27A", IsUnique = true)]
public partial class Sysuser
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    [StringLength(50)]
    public string? DiaChi { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(500)]
    public string? Avatar { get; set; }

    [StringLength(100)]
    public string Passwords { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string CuaHangId { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? RoleId { get; set; }

    [ForeignKey("CuaHangId")]
    [InverseProperty("Sysusers")]
    public virtual Cuahang CuaHang { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    [ForeignKey("RoleId")]
    [InverseProperty("Sysusers")]
    public virtual Sysrole? Role { get; set; }
}
