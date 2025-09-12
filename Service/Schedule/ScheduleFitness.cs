using GeneticSharp;

namespace QuanLySinhVien.Service.Schedule
{
    public class ScheduleFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();
            var schedules = new List<schedule>();

            // gom tất cả lớp trong các gene về 1 list duy nhất
            foreach (var g in genes)
            {
                if (g.Value is List<schedule> list)
                {
                    schedules.AddRange(list);
                }
                else if (g.Value is schedule single)
                {
                    schedules.Add(single);
                }
            }

            double score = 1000; // điểm khởi đầu

            // Ràng buộc
            foreach (var pair in schedules)
            {
                
                if ((pair.Slot == 2 || pair.Slot == 4 || pair.Slot == 6) && pair.Classes.SoTiet > 3  )
                {
                    score -= 100;
                }


                // 3) Kiểm tra trùng phòng
                foreach (var other in schedules)
                {
                    if (pair == other) continue;

                    if (pair.PhongHoc == other.PhongHoc && pair.Slot == other.Slot)
                    {
                        score -= 100;
                    }
                    
                    //Kiểm tra các môn có số tiết > 3 sẽ phải lấy phòng của ca sau 
                    if (pair.Classes.SoTiet > 3 && (pair.Slot + 1) == other.Slot)
                    {
                        score -= 100;
                    }
                }
            }

            return Math.Max(0, score);
        }

    }
}
