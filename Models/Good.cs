using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Good
{
    public string GoodId { get; set; } = null!;

    public string GoodName { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public virtual ICollection<RecorDetail> RecorDetails { get; set; } = new List<RecorDetail>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
