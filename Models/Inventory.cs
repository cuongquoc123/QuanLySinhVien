using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("inventory", Schema = "management")]
public partial class Inventory
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string InventoryId { get; set; } = null!;

    [StringLength(100)]
    public string Status { get; set; } = null!;

    [StringLength(100)]
    public string Addr { get; set; } = null!;

    [Column("StoreID")]
    [StringLength(10)]
    [Unicode(false)]
    public string StoreId { get; set; } = null!;

    [InverseProperty("Inventory")]
    public virtual ICollection<Grn> Grns { get; set; } = new List<Grn>();

    [ForeignKey("StoreId")]
    [InverseProperty("Inventories")]
    public virtual Store Store { get; set; } = null!;
}
