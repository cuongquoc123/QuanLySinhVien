using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Store
{
    public string StoreId { get; set; } = null!;

    public string StoreName { get; set; } = null!;

    public string StoreAddr { get; set; } = null!;

    public string? PhoneNum { get; set; }

    public string? StoreStatus { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Sysuser> Sysusers { get; set; } = new List<Sysuser>();
}
