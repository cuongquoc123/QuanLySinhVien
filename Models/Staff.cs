using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("staff", Schema = "management")]
public partial class Staff
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string StaffId { get; set; } = null!;

    [StringLength(11)]
    [Unicode(false)]
    public string IdNumber { get; set; } = null!;

    [StringLength(50)]
    public string StaffName { get; set; } = null!;

    [StringLength(50)]
    public string StaffAddr { get; set; } = null!;

    public DateOnly? DoB { get; set; }

    [Column(TypeName = "money")]
    public decimal Salary { get; set; }

    [Column(TypeName = "money")]
    public decimal Bonus { get; set; }

    [StringLength(500)]
    public string Avatar { get; set; } = null!;

    [StringLength(100)]
    public string? Status { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? StoreId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("Staff")]
    public virtual Sysrole Role { get; set; } = null!;

    [ForeignKey("StoreId")]
    [InverseProperty("Staff")]
    public virtual Store? Store { get; set; }

    [InverseProperty("Staff")]
    public virtual ICollection<Sysuser> Sysusers { get; set; } = new List<Sysuser>();
}
