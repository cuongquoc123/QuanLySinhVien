using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public decimal Price { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Img { get; set; }

    public string? Status { get; set; }

    public string? Decription { get; set; }

    public string SubcategoryId { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual SubCategory Subcategory { get; set; } = null!;
}
