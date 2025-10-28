using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("category")]
public partial class Category
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string CategoryId { get; set; } = null!;

    [StringLength(10)]
    public string? CategoryName { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
