namespace QuanLySinhVien.Service.CheckFace
{

    public interface IcheckFace
    {
        Task<int> CheckFaceAsync(string imgPath1, string imgPath2);
    }
}