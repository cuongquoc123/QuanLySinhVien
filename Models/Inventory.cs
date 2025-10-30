using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Inventory
{
    public string InventoryId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Addr { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public virtual ICollection<Grn> Grns { get; set; } = new List<Grn>();

    public virtual Store Store { get; set; } = null!;
}
