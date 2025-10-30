using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Grndetail
{
    public string GoodId { get; set; } = null!;

    public int ReStock { get; set; }

    public string GrnId { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;

    public virtual Grn Grn { get; set; } = null!;
}
