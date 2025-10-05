namespace QuanLySinhVien.DTOS.Respone
{
    public class ErrorRespone
    {

        public int status { get; set; }
        public string message { get; set; }
        public string Error { get; set; }
        public DateTime Time { get; set; }
        public string path { get; set; }

        public ErrorRespone()
        {
            status = 500;
            message = "";
            Error = "";
            Time = DateTime.Now;
            path = "";
        }
    }
}