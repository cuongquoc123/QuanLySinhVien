using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Gondetail
{
    public string Gonid { get; set; } = null!;

    public string GoodId { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Gon Gon { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;
}
