using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("GiangVien")]
public partial class GiangVien
{
    [Key]
    [Column("MaGV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaGv { get; set; } = null!;

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [Column("SDT")]
    [StringLength(20)]
    public string? Sdt { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MaKhoa { get; set; } = null!;

    [InverseProperty("MaGvNavigation")]
    public virtual ICollection<LopHocPhan> LopHocPhans { get; set; } = new List<LopHocPhan>();

    [ForeignKey("MaKhoa")]
    [InverseProperty("GiangViens")]
    public virtual Khoa MaKhoaNavigation { get; set; } = null!;
}
