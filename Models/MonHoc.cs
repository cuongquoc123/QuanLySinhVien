using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("MonHoc")]
public partial class MonHoc
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaMon { get; set; } = null!;

    [StringLength(100)]
    public string TenMon { get; set; } = null!;

    public int? SoTinChi { get; set; }

    public int SoTiet { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MaNganh { get; set; } = null!;

    [InverseProperty("MaMonNavigation")]
    public virtual ICollection<LopHocPhan> LopHocPhans { get; set; } = new List<LopHocPhan>();

    [ForeignKey("MaNganh")]
    [InverseProperty("MonHocs")]
    public virtual Nganh MaNganhNavigation { get; set; } = null!;
}
