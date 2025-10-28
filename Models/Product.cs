using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[Table("products")]
public partial class Product
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string ProductId { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [Column("IMG")]
    [StringLength(500)]
    public string? Img { get; set; }

    [StringLength(100)]
    public string? Status { get; set; }

    [Column(TypeName = "text")]
    public string? Decription { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string SubcategoryId { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("SubcategoryId")]
    [InverseProperty("Products")]
    public virtual SubCategory Subcategory { get; set; } = null!;
}
