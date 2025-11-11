using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public string Status { get; set; } = null!;

    public string Addr { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public virtual ICollection<Inventoryrecord> Inventoryrecords { get; set; } = new List<Inventoryrecord>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual Store Store { get; set; } = null!;
}
