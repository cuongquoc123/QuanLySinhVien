using GeneticSharp;

namespace QuanLySinhVien.Service.Schedule
{
    public class FormatSchedule
    {

        public string DayOfWeek { get; set; }
        public List<Schedulee> schedules { get; set; }

        public FormatSchedule(string DayOfWeek, List<Schedulee> schedules)
        {
            this.DayOfWeek = DayOfWeek;
            this.schedules = schedules;
        }

        public FormatSchedule()
        {
            this.DayOfWeek = string.Empty;
            this.schedules = new List<Schedulee>();
        }

        public static List<FormatSchedule> formatSchedule(Gene[][] genes)
        {
            var FormatList = new List<FormatSchedule>();
            string[] days = { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ Nhật" };

            for (int dayIndex = 0; dayIndex < days.Count(); dayIndex++)
            {
                var DaySchedule = genes[dayIndex].SelectMany(gens => (List<Schedulee>)gens.Value).ToList();
                var GroupDataBySlot = DaySchedule.GroupBy(g => g.Slot);

                foreach (var GroupData in GroupDataBySlot)
                {
                    FormatList.Add(new FormatSchedule
                    {
                        DayOfWeek = days[dayIndex].ToString(),
                        schedules = GroupData.ToList()
                    });
                }
            }
            return FormatList;
        }
    }
}