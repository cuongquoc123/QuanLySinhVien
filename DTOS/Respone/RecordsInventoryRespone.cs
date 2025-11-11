namespace QuanLySinhVien.DTOS.Respone
{
    public class RecordsInventoryRespone
    {
        public string RecordId { get; set; } = null!;
        public DateTime AdmissionDate { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; } = null!;
    }
}