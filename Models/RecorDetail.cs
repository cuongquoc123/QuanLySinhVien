using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class RecorDetail
{
    public string GoodId { get; set; } = null!;

    public int Quantity { get; set; }

    public string RecordsId { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;

    public virtual Inventoryrecord Records { get; set; } = null!;
}
