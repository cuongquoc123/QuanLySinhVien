using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime RecivingDate { get; set; }

    public DateTime UpdateStatusDate { get; set; }

    public DateTime? CompleteDate { get; set; }

    public string? CustomerId { get; set; }

    public int? SysUserId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Sysuser? SysUser { get; set; }
}
