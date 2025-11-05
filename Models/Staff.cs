using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Staff
{
    public string StaffId { get; set; } = null!;

    public string IdNumber { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string StaffAddr { get; set; } = null!;

    public DateOnly? DoB { get; set; }

    public string? Email { get; set; }

    public string? PhoneNum { get; set; }

    public string? Gender { get; set; }

    public decimal Salary { get; set; }

    public decimal Bonus { get; set; }

    public string Avatar { get; set; } = null!;

    public string? Status { get; set; }

    public string? StoreId { get; set; }

    public string RoleId { get; set; } = null!;

    public virtual Sysrole Role { get; set; } = null!;

    public virtual Store? Store { get; set; }

    public virtual ICollection<Sysuser> Sysusers { get; set; } = new List<Sysuser>();
}
