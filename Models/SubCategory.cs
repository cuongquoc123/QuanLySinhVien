using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class SubCategory
{
    public string SubCategoryId { get; set; } = null!;

    public string SubCategoryName { get; set; } = null!;

    public string CategoryId { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
