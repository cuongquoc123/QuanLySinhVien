namespace QuanLySinhVien.MidWare.Filter
{
    public class CustomError : Exception
    {
        public int status { get; set; }
        public string Error { get; set; }

        public CustomError(int status, string error, string message) : base(message)
        {
            this.status = status;
            this.Error = error;
        }
    }
}