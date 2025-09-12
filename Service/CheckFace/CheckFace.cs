using System.Diagnostics;

namespace QuanLySinhVien.Service.CheckFace
{


    public class CheckFace : IcheckFace
    {
        public async Task<int> CheckFaceAsync(string imgPath1, string imgPath2)
        {
            //Cấu hình đường dẫn python và scrypt
            string pythonPath = "/opt/homebrew/bin/python3"; //Cập nhật đường dẫn python đúng
            string pythonScrypt = "/Users/cps/Desktop/Yolo_face/Check_faced.py"; //Đảm bảo đường dẫn tuyệt đối cho file scrypt 

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = $"{pythonScrypt} \"{imgPath1}\"  \"{imgPath2}\"";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            try
            {
                using (Process process = Process.Start(start))
                {
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();

                    await Task.WhenAll(outputTask, errorTask);

                    string output = outputTask.Result ?? string.Empty;
                    string error = errorTask.Result ?? string.Empty;

                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Python exited with code {process.ExitCode}.\nError: {error}\nOutput: {output}");
                    }

                    //File python đã chạy thành công 
                    if (!string.IsNullOrEmpty(error))
                    {
                        //Log lỗi python nhưng không phải là lỗi API 
                        System.Console.WriteLine($"Python produced warnings:\n{error}");
                    }
                    //Kiểm tra kết quả có khớp không 
                    if (output.Contains("Ket_qua_khop"))
                    {
                        return 200;
                    }
                    else throw new UnauthorizedAccessException("Kết quả không khớp, xác thực thất bại");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Không thể khởi chạy Pyhon");
            }
        }
    }
}