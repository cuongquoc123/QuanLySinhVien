using System.Text;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.HTMLRaw
{
    public class HTMLService : IHtmService
    {
        private readonly MyDbContext context;

        public HTMLService(MyDbContext context)
        {
            this.context = context;
        }
        public string HoaDonHTMl(String Madon)
        {
            // ... (phần code truy vấn dữ liệu của bạn giữ nguyên) ...
            var donHang = context.Donhangs
                                 .Include(dh => dh.User)
                                 .FirstOrDefault(dh => dh.MaDon == Madon);

            if (donHang == null)
            {
                throw new KeyNotFoundException("Bill not exists");
            }

            var ChiTietDonHangs = context.ChiTietDonHangs
                                         .Include(ctdh => ctdh.MaSpNavigation)
                                         .Where(x => x.MaDon == donHang.MaDon)
                                         .ToList();

            if (ChiTietDonHangs == null || !ChiTietDonHangs.Any())
            {
                throw new KeyNotFoundException("Fake bills");
            }
            var viVNCulture = new System.Globalization.CultureInfo("vi-VN");
            decimal thanhTien = 0;
            var Table = new StringBuilder();
            Table.AppendLine(@"
    <thead>
        <tr class='heading'>
            <td>Sản phẩm</td>
            <td class='text-right'>Đơn giá</td>
            <td class='text-center'>Số lượng</td>
            <td class='text-right'>Thành tiền</td>
        </tr>
    </thead>
    <tbody>");

            foreach (var item in ChiTietDonHangs)
            {
                var donGia = item.MaSpNavigation?.DonGia ?? 0;
                var tenSp = item.MaSpNavigation?.TenSp ?? "Sản phẩm không xác định";
                var thanhTienItem = item.SoLuong * donGia;

                Table.AppendLine($@"
        <tr class='item'>
            <td>{tenSp}</td>
            <td class='text-right'>{donGia.ToString("N0", viVNCulture)}đ</td>
            <td class='text-center'>{item.SoLuong}</td>
            <td class='text-right'>{thanhTienItem.ToString("N0", viVNCulture)}đ</td>
        </tr>");

                thanhTien += item.SoLuong * donGia;
            }
            Table.AppendLine(@"</tbody>");

           
            var thue = thanhTien * 0.1m; // Giả sử VAT 10%
            var tamTinh = thanhTien - thue;

            // PHẦN TEMPLATE HTML MỚI
            string HTMlContent = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8' />
            <title>Hóa Đơn #{donHang.MaDon}</title>
            <style>
                body {{
                    font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;
                    color: #555;
                    background: #FFF;
                    font-size: 14px;
                }}
                .invoice-box {{
                    max-width: 800px;
                    margin: auto;
                    padding: 30px;
                    border: 1px solid #eee;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                }}
                .invoice-box table {{
                    width: 100%;
                    line-height: inherit;
                    text-align: left;
                    border-collapse: collapse;
                }}
                .invoice-box table td {{
                    padding: 5px;
                    vertical-align: top;
                }}
                .invoice-box table tr td:nth-child(2) {{
                    text-align: right;
                }}
                .invoice-box .header {{
                    margin-bottom: 20px;
                }}
                .invoice-box .header .logo {{
                    width: 100%;
                    max-width: 200px;
                }}
                .invoice-box .header .company-details {{
                    text-align: right;
                }}
                .invoice-box .invoice-details td {{
                    padding-bottom: 20px;
                }}
                .invoice-box .item-table table th, .invoice-box .item-table table td {{
                    border-bottom: 1px solid #eee;
                    padding: 10px 5px;
                }}
                .invoice-box .item-table table .heading td {{
                    background: #eee;
                    border-bottom: 1px solid #ddd;
                    font-weight: bold;
                }}
                .invoice-box .item-table table .item td {{
                    border-bottom: 1px solid #eee;
                }}
                .invoice-box .totals table {{
                    width: 50%;
                    float: right;
                }}
                .invoice-box .totals table .total td:nth-child(2) {{
                    border-top: 2px solid #eee;
                    font-weight: bold;
                }}
                .text-right {{
                    text-align: right !important;
                }}
                .text-center {{
                    text-align: center !important;
                }}

                /* Tối ưu cho việc in ấn */
                @media print {{
                    .invoice-box {{
                        box-shadow: none;
                        border: 0;
                    }}
                }}
            </style>
        </head>
        <body>
            <div class='invoice-box'>
                <table class='header' cellpadding='0' cellspacing='0'>
                    <tr>
                        <td>
                            <img src='https://i.imgur.com/vU2p3s2.png' alt='Company Logo' class='logo' />
                        </td>
                    </tr>
                </table>

                <table class='invoice-details' cellpadding='0' cellspacing='0'>
                    <tr>
                        <td>
                            <strong>Khách hàng</strong><br />
                            Khách lẻ
                        </td>
                        <td class='text-right'>
                            <strong>Hóa đơn #: {donHang.MaDon}</strong><br />
                            Ngày tạo: {donHang.NgayNhan.ToString("dd/MM/yyyy")}<br />
                            Nhân viên: {donHang.User?.UserName ?? "Không xác định"}
                        </td>
                    </tr>
                </table>

                <div class='item-table'>
                    <table>
                        {Table.ToString()}
                    </table>
                </div>

                <div class='totals'>
                    <table cellpadding='0' cellspacing='0'>
                        <tr>
                            <td>Tạm tính</td>
                            <td class='text-right'>{tamTinh.ToString("N0",viVNCulture)}đ</td>
                        </tr>
                        <tr>
                            <td>Thuế VAT(10%)</td>
                            <td class='text-right'>{thue.ToString("N0",viVNCulture)}đ</td>
                        </tr>
                        <tr class='total'>
                            <td><strong>Tổng cộng</strong></td>
                            <td class='text-right'><strong>{thanhTien.ToString("N0",viVNCulture)}đ</strong></td>
                        </tr>
                    </table>
                </div>
                 <div style='clear: both;'></div>
            </div>
        </body>
        </html>";
            return HTMlContent;
        }
    }
}