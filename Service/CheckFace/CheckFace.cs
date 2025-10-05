using System.Diagnostics;
using System.Net;
using QuanLySinhVien.MidWare.Filter;

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
                using (Process process = Process.Start(start)!)
                {
                    if (process == null)
                    {
                        throw new InvalidOperationException("Không thể khởi tạo tiến trình Python");
                    }
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

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Python produced warnings:\n{error}");
                    }

                    if (output.Contains("Ket_qua_khop"))
                    {
                        return 200;
                    }

                    throw new UnauthorizedAccessException("Kết quả không khớp, xác thực thất bại");
                }
            }
            catch (Exception e)
            {
                throw new CustomError(500, "Internal Server Error", message: e.Message);
            }
        }
    }
}