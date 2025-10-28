using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("store")]
public partial class Store
{
    [Key]
    [Column("StoreID")]
    [StringLength(10)]
    [Unicode(false)]
    public string StoreId { get; set; } = null!;

    [StringLength(100)]
    public string StoreName { get; set; } = null!;

    [StringLength(100)]
    public string StoreAddr { get; set; } = null!;

    [StringLength(11)]
    [Unicode(false)]
    public string? PhoneNum { get; set; }

    [Column("Store_Status")]
    [StringLength(100)]
    public string? StoreStatus { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [InverseProperty("Store")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Store")]
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    [InverseProperty("Store")]
    public virtual ICollection<Sysuser> Sysusers { get; set; } = new List<Sysuser>();
}
