using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("username")]
    [StringLength(10)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("roleId")]
    [StringLength(3)]
    [Unicode(false)]
    public string? RoleId { get; set; }

    [Column("passwords")]
    [StringLength(50)]
    public string? Passwords { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }
}
