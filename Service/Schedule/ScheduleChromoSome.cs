using GeneticSharp;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.Schedule
{
    public class ScheduleChromoSome : ChromosomeBase
    {
        private readonly int _slots;
        private readonly List<LopHocPhan> _classes;

        private readonly List<string> phongHoc;

        public ScheduleChromoSome(int Slots, List<LopHocPhan> classrooms, List<string> PhongHoc) : base(Slots)
        {
            _slots = Slots; //số lượng tiết học của 1 ngày => tuần =  số ca học 1 ngày * 7 ví dụ 1 ngày 5 ca => 1 tuần = 7 * 5 = 35 => slots 35
            _classes = classrooms;
            phongHoc = PhongHoc;
            
            //Vòng lập tạo 
            for (int i = 0; i < _slots; i++)
            {
                var schedulesInSlot = new List<schedule>();

                // Ví dụ: random số lớp trong 1 slot (1–3 lớp chẳng hạn)
                int numberOfClasses = PhongHoc.Count;

                for (int j = 0; j < 3; j++)
                {
                    var randomClass = _classes[RandomizationProvider.Current.GetInt(0, _classes.Count)];
                    var randomPH = phongHoc[RandomizationProvider.Current.GetInt(0, phongHoc.Count)];

                    schedulesInSlot.Add(new schedule
                    {
                        Slot = i + 1,
                        Classes = randomClass,
                        PhongHoc = randomPH,
                        Sotiet = randomClass.SoTiet
                    });
                }
                ReplaceGene(i, new Gene(schedulesInSlot));
            }

        }


        public override IChromosome CreateNew()
        {
            return new ScheduleChromoSome(_slots, _classes, phongHoc);
        }

        //Hàm dùng để đột biến trong thuật toán di truyền 
        public override Gene GenerateGene(int geneIndex)
        {

            var schedulesInSlot = new List<schedule>();

            int numberOfClasses = phongHoc.Count;

            for (int j = 0; j < numberOfClasses; j++)
            {
                var randomClass = _classes[RandomizationProvider.Current.GetInt(0, _classes.Count)];
                var randomPH = phongHoc[RandomizationProvider.Current.GetInt(0, phongHoc.Count)];

                schedulesInSlot.Add(new schedule
                {
                    Slot = geneIndex + 1,
                    Classes = randomClass,
                    PhongHoc = randomPH,
                    Sotiet = randomClass.SoTiet
                });
            }

            return new Gene(schedulesInSlot);
        }
    }
}