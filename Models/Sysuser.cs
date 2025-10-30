using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Sysuser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? StaffId { get; set; }

    public string StoreId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Sysrole Role { get; set; } = null!;

    public virtual Staff? Staff { get; set; }

    public virtual Store Store { get; set; } = null!;
}
