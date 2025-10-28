using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models;

[PrimaryKey("OrderId", "ProductId")]
[Table("order_detail")]
public partial class OrderDetail
{
    public int Quantity { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string OrderId { get; set; } = null!;

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string ProductId { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
