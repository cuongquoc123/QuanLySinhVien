using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("LopHanhChinh")]
public partial class LopHanhChinh
{
    [Key]
    [Column("MaLopHC")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaLopHc { get; set; } = null!;

    [Column("TenLopHC")]
    [StringLength(100)]
    public string TenLopHc { get; set; } = null!;

    public int? SiSo { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MaNganh { get; set; } = null!;

    [ForeignKey("MaNganh")]
    [InverseProperty("LopHanhChinhs")]
    public virtual Nganh MaNganhNavigation { get; set; } = null!;

    [InverseProperty("MaLopHcNavigation")]
    public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
}
