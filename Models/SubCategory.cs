using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("sub_category")]
public partial class SubCategory
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string SubCategoryId { get; set; } = null!;

    [StringLength(50)]
    public string SubCategoryName { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string CategoryId { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("SubCategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Subcategory")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
