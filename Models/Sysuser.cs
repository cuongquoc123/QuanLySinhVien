using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sysuser")]
[Index("UserName", Name = "UQ__sysuser__C9F28456FE94623B", IsUnique = true)]
public partial class Sysuser
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    public string Passwords { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? RoleId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    [ForeignKey("RoleId")]
    [InverseProperty("Sysusers")]
    public virtual Sysrole? Role { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Sysuser")]
    public virtual Staff User { get; set; } = null!;
}
