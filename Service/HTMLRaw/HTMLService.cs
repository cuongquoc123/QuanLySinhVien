using System.Text;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.HTMLRaw
{
    public class HTMLService : IHtmService
    {

        public string HoaDonHTMl(Donhang donhang)
        {
            var Table = new StringBuilder();
            Table.AppendLine(@"
            <thead>
                <tr>
                    <th>Tên Sản Phẩm</th>
                    <th class='text-right'>Đơn giá</th>
                    <th class='text-right'>SL</th>
                    <th class='text-right'>Thành tiền</th>
                </tr>
            </thead>
            <tbody>");

            foreach (var item in donhang.ChiTietDonHangs)
            {
                Table.AppendLine($@"
                <tr>
                    <td>{item.MaSpNavigation.TenSp}</td>
                    <td class='text-right'>{item.MaSpNavigation.DonGia}</td>
                    <td class='text-right'>{item.SoLuong}</td>
                    <td class='text-right'>{item.SoLuong * item.MaSpNavigation.DonGia}</td>
                </tr>");
            }
            Table.AppendLine(@"</tbody>");

            string HTMlContent = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8' />
                        <title>Hóa Đơn #{donhang.MaDon}</title>
                        <style>
                            body {{ font-family: 'Times New Roman', Times, serif; font-size: 14px; margin: 0; padding: 20px; }}
                            .invoice-box {{ max-width: 800px; margin: auto; border: 1px solid #eee; padding: 30px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.15); }}
                            h1 {{ text-align: center; color: #333; }}
                            table {{ width: 100%; line-height: inherit; text-align: left; border-collapse: collapse; }}
                            .invoice-info {{ margin-bottom: 20px; }}
                            .invoice-info td {{ padding-bottom: 20px; }}
                            .item-table th, .item-table td {{ border-bottom: 1px solid #eee; padding: 8px; }}
                            .item-table th {{ background: #eee; }}
                            .text-right {{ text-align: right; }}
                            .total-row {{ font-weight: bold; }}
                        </style>
                    </head>
                    <body>
                        <div class='invoice-box'>
                            <h1 style='color: #2c3e50;'>HÓA ĐƠN BÁN HÀNG</h1>
                            
                            <table cellpadding='0' cellspacing='0' class='invoice-info'>
                                <tr>
                                    <td style='width: 50%;'>
                                        <strong>Công ty Cổ phần Đối tác lập trình</strong><br>
                                        Địa chỉ: Số 123, Đường Lập Trình, TP. HCM<br>
                                        Email: support@partner.com
                                    </td>
                                    <td class='text-right'>
                                        <strong>Hóa đơn #: {donhang.MaDon}</strong><br>
                                        Ngày phát hành: {donhang.NgayNhan.ToString("dd/MM/yyyy")}<br>
                                    </td>
                                </tr>
                            </table>
                            
                            <table cellpadding='0' cellspacing='0' class='item-table'>
                                {Table.ToString()}
                            </table>

                            <table style='width: 40%; float: right; margin-top: 20px;'>
                                <tr>
                                    <td>Tạm tính:</td>
                                    <td class='text-right'>{(donhang.ThanhTien * 90 / 100).ToString("N2")} VNĐ</td>
                                </tr>
                                <tr>
                                    <td>Thuế 10%: </td>
                                    <td class='text-right'>{(donhang.ThanhTien * 10 / 100).ToString("N2")} VNĐ</td>
                                </tr>
                                <tr class='total-row'>
                                    <td>TỔNG CỘNG:</td>
                                    <td class='text-right'>{donhang.ThanhTien.ToString("N2")} VNĐ</td>
                                </tr>
                            </table>
                            <div style='clear: both;'></div>

                            <p style='margin-top: 50px; text-align: center; font-style: italic;'>
                                Cảm ơn quý khách đã tin tưởng dịch vụ của chúng tôi!
                            </p>
                        </div>
                    </body>
                    </html>";
                return HTMlContent;
        }
    }
}