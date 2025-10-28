using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("goods", Schema = "management")]
public partial class Good
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string GoodId { get; set; } = null!;

    [StringLength(100)]
    public string GoodName { get; set; } = null!;

    [StringLength(20)]
    public string UnitName { get; set; } = null!;

    [InverseProperty("Good")]
    public virtual ICollection<Grndetail> Grndetails { get; set; } = new List<Grndetail>();
}
