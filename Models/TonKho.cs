using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Keyless]
[Table("TonKho")]
public partial class TonKho
{
    [StringLength(10)]
    [Unicode(false)]
    public string MaKho { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MaNguyenLieu { get; set; } = null!;

    public int SoLuongTon { get; set; }

    [ForeignKey("MaKho")]
    public virtual Kho MaKhoNavigation { get; set; } = null!;

    [ForeignKey("MaNguyenLieu")]
    public virtual Nguyenlieu MaNguyenLieuNavigation { get; set; } = null!;
}
