using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Sysrole
{
    public string RoleId { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Sysuser> Sysusers { get; set; } = new List<Sysuser>();
}
