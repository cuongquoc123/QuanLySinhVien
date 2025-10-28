using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[PrimaryKey("GoodId", "GrnId")]
[Table("GRNDetail", Schema = "management")]
public partial class Grndetail
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string GoodId { get; set; } = null!;

    public int ReStock { get; set; }

    [Key]
    [Column("GRN_ID")]
    [StringLength(10)]
    [Unicode(false)]
    public string GrnId { get; set; } = null!;

    [ForeignKey("GoodId")]
    [InverseProperty("Grndetails")]
    public virtual Good Good { get; set; } = null!;

    [ForeignKey("GrnId")]
    [InverseProperty("Grndetails")]
    public virtual Grn Grn { get; set; } = null!;
}
