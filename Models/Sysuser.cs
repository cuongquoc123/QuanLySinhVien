using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sysuser", Schema = "management")]
[Index("UserName", Name = "UQ__sysuser__C9F2845615550E20", IsUnique = true)]
public partial class Sysuser
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? StaffId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string StoreId { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [InverseProperty("SysUser")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("RoleId")]
    [InverseProperty("Sysusers")]
    public virtual Sysrole Role { get; set; } = null!;

    [ForeignKey("StaffId")]
    [InverseProperty("Sysusers")]
    public virtual Staff? Staff { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("Sysusers")]
    public virtual Store Store { get; set; } = null!;
}
