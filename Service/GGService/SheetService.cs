
using DotNetEnv;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace QuanLySinhVien.Service.GGService
{
    public class SheetService : ISheetService
    {
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private static readonly string PathToCredential = Env.GetString("GGSheetAPIKey");
        private static readonly string ApplicationName = "QuanLyChuoiCuaHang";
        private readonly string _spreadsheetId = Env.GetString("SheetId");
        public async Task<List<List<string>>> StoreReview(string storeId)
        {
            //Danh sách chứa kết quả cuối cùng
            var data = new List<List<string>>();

            GoogleCredential credential;
            // Đọc file Credential
            using (var stream = new FileStream(PathToCredential, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            // Tạo dịch vụ Google Sheets
            var Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            // Chỉ định Sheet và phạm vi cần đọc (Cột A đến F)
            var range = "sheet1!A:F";

            try
            {
                // Gửi yêu cầu đọc dữ liệu
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    Service.Spreadsheets.Values.Get(_spreadsheetId, range);

                ValueRange respone = await request.ExecuteAsync();
                IList<IList<object>> values = respone.Values;
                System.Console.WriteLine( values.Count);
                
                {
                    // Bỏ qua dòng đầu tiên (tiêu đề) và bắt đầu xử lý dữ liệu
                    // Dùng LINQ để lọc
                    var query = from row in values.Skip(1)
                                where row.Count >= 6 && row[5] != null &&
                                // Điều kiện lọc:
                                // 1. Dòng phải có đủ 6 cột (để không bị lỗi khi truy cập cột F)
                                // 2. Giá trị ở cột F (index 5) không được rỗng
                                // 3. Giá trị ở cột F phải khớp với mã cửa hàng cần lọc 
                                row[5].ToString().Equals(storeId)
                                select row.Select(cell => cell.ToString()).ToList();
                    data = query.ToList();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return data;
        }
    }
}