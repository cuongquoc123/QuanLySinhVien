using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Recordtype
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Inventoryrecord> Inventoryrecords { get; set; } = new List<Inventoryrecord>();
}
