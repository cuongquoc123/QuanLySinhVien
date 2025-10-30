using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class OrderDetail
{
    public int Quantity { get; set; }

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
