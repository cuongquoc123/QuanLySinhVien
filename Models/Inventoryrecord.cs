using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Inventoryrecord
{
    public string RecordsId { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public int InventoryId { get; set; }

    public int TypeId { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;

    public virtual ICollection<RecorDetail> RecorDetails { get; set; } = new List<RecorDetail>();

    public virtual Recordtype Type { get; set; } = null!;
}
