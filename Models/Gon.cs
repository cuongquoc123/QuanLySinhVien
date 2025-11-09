using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Gon
{
    public string Gonid { get; set; } = null!;

    public int InventoryId { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual ICollection<Gondetail> Gondetails { get; set; } = new List<Gondetail>();

    public virtual Inventory Inventory { get; set; } = null!;
}
