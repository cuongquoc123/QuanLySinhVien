using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sysrole")]
public partial class Sysrole
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
