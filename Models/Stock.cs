using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Keyless]
[Table("Stock", Schema = "management")]
public partial class Stock
{
    [StringLength(10)]
    [Unicode(false)]
    public string InventoryId { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string GoodId { get; set; } = null!;

    [StringLength(100)]
    public string Status { get; set; } = null!;

    public int InStock { get; set; }

    [ForeignKey("GoodId")]
    public virtual Good Good { get; set; } = null!;

    [ForeignKey("InventoryId")]
    public virtual Inventory Inventory { get; set; } = null!;
}
