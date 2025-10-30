using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Stock
{
    public string InventoryId { get; set; } = null!;

    public string GoodId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int InStock { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Inventory Inventory { get; set; } = null!;
}
