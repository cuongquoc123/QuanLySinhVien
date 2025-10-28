using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("GRN", Schema = "management")]
public partial class Grn
{
    [Key]
    [Column("GRN_ID")]
    [StringLength(10)]
    [Unicode(false)]
    public string GrnId { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? InventoryId { get; set; }

    [InverseProperty("Grn")]
    public virtual ICollection<Grndetail> Grndetails { get; set; } = new List<Grndetail>();

    [ForeignKey("InventoryId")]
    [InverseProperty("Grns")]
    public virtual Inventory? Inventory { get; set; }
}
