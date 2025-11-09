using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Good
{
    public string GoodId { get; set; } = null!;

    public string GoodName { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public virtual ICollection<Gondetail> Gondetails { get; set; } = new List<Gondetail>();

    public virtual ICollection<Grndetail> Grndetails { get; set; } = new List<Grndetail>();
}
