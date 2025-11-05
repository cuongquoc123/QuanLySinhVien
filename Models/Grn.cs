using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Grn
{
    public string GrnId { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public int? InventoryId { get; set; }

    public virtual ICollection<Grndetail> Grndetails { get; set; } = new List<Grndetail>();

    public virtual Inventory? Inventory { get; set; }
}
