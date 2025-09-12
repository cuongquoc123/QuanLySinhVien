using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("Nganh")]
public partial class Nganh
{
    [Key]
    [StringLength(10)]
    public string MaNganh { get; set; } = null!;

    [StringLength(100)]
    public string TenNganh { get; set; } = null!;

    [InverseProperty("MaNganhNavigation")]
    public virtual ICollection<LopHanhChinh> LopHanhChinhs { get; set; } = new List<LopHanhChinh>();

    [InverseProperty("MaNganhNavigation")]
    public virtual ICollection<MonHoc> MonHocs { get; set; } = new List<MonHoc>();
}
