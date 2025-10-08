using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("staff")]
public partial class Staff
{
    [Key]
    [Column("CCCD")]
    [StringLength(11)]
    [Unicode(false)]
    public string Cccd { get; set; } = null!;

    [StringLength(50)]
    public string Ten { get; set; } = null!;

    [StringLength(50)]
    public string DiaChi { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    [StringLength(50)]
    public string? Vtri { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? Luong { get; set; }

    [StringLength(500)]
    public string Avatar { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? CuaHangId { get; set; }

    [ForeignKey("CuaHangId")]
    [InverseProperty("Staff")]
    public virtual Cuahang? CuaHang { get; set; }
}
