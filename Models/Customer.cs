using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("Customer")]
[Index("UserName", Name = "UQ__Customer__C9F284560EF52563", IsUnique = true)]
public partial class Customer
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string CustomerId { get; set; } = null!;

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    public string Passwords { get; set; } = null!;

    [Column("statusC")]
    [StringLength(50)]
    public string? StatusC { get; set; }

    [InverseProperty("Customer")]
    public virtual CustomerDetail? CustomerDetail { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
}
