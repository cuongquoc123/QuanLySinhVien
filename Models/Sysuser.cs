using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sysuser")]
[Index("UserName", Name = "UQ__sysuser__C9F284563D75F844", IsUnique = true)]
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

    [ForeignKey("UserId")]
    [InverseProperty("Sysuser")]
    public virtual Staff User { get; set; } = null!;
}
