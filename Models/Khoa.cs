using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("Khoa")]
public partial class Khoa
{
    [Key]
    [StringLength(10)]
    public string MaKhoa { get; set; } = null!;

    [StringLength(100)]
    public string TenKhoa { get; set; } = null!;

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<GiangVien> GiangViens { get; set; } = new List<GiangVien>();
}
