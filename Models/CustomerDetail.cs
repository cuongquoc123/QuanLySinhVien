using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("CustomerDetail")]
[Index("Email", Name = "UQ__Customer__A9D10534CBF5AC9F", IsUnique = true)]
[Index("Sdt", Name = "UQ__Customer__CA1930A5A690E4FE", IsUnique = true)]
public partial class CustomerDetail
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string CustomerId { get; set; } = null!;

    [StringLength(50)]
    public string? TenKhach { get; set; }

    [Column("CCCD")]
    [StringLength(11)]
    [Unicode(false)]
    public string? Cccd { get; set; }

    [Column("SDT")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Sdt { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Avatar { get; set; }

    [StringLength(100)]
    public string? DiaChi { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerDetail")]
    public virtual Customer Customer { get; set; } = null!;
}
