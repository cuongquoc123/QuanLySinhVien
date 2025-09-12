using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("roles")]
public partial class Role
{
    [Key]
    [Column("roleId")]
    [StringLength(3)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [Column("roleName")]
    [StringLength(30)]
    public string? RoleName { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
