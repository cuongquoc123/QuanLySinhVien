using GeneticSharp;

using QuanLySinhVien.Models;
namespace QuanLySinhVien.Service.Schedule
{


    public class GA : IGAServices
    {
        public GA()
        {
        }

        public Gene[] Ga(int slot, List<LopHocPhan> classrooms, List<string> PhongHoc)
        {

            //Tạo nhiễm sắc thể đầu tiên với số tiết học trong tuần , mỗi tiết là 1 Giáo viên dạy 1 Lớp 
            var chromosome = new ScheduleChromoSome(slot, classrooms, PhongHoc);

            //Cộng trừ điểm để đánh giá 1 lịch dạy học 
            var fitness = new ScheduleFitness();

            //Tạo ra quần thể với 20 là số lượng cá thể nhỏ nhất và 50 là lớn nhất, Còn chromosome là tạo lịch ngẫu nhiên
            var population = new Population(20, 50, chromosome);
            var ga = new GeneticAlgorithm(
                population,
                fitness,
                new TournamentSelection(), // Chọn ra lịch tốt nhất từ 1 nhóm nhỏ để sinh sản 
                new UniformCrossover(), // Trộn gen của bố và mẹ 
                new UniformMutation() // Thay đổi ngẫu nhiên 1 số gen để tạo sự đa dạng 
            );

            ga.Termination = new GenerationNumberTermination(100);

            ga.Start();

            var Best = ga.BestChromosome.GetGenes();

            return Best;
        }
    }
}