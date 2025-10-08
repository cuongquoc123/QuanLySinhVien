using GeneticSharp;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;

using QuanLySinhVien.Service.Schedule;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller
{
    [ApiController]
    [Route("/public")]
    public class ScheduleController : ControllerBase
    {
        private readonly ISqLServices sqLServices;
        private readonly ILogger<ScheduleController> logger;
        private readonly IGAServices gAServices;
        private readonly MyDbContext myDbContext;

        public ScheduleController(MyDbContext context, IGAServices gAServices, ILogger<ScheduleController> logger, ISqLServices sqLServices)
        {
            myDbContext = context;
            this.gAServices = gAServices;
            this.logger = logger;
            this.sqLServices = sqLServices;
        }

        [HttpPost("LapLich/{HocKy}/{NamHoc}")]
        public async Task<IActionResult> Schedule(string HocKy,string NamHoc)
        {
            List<LopHocPhan> classes = myDbContext.LopHocPhans.Where(l => l.HocKy == HocKy && l.NamHoc == NamHoc).ToList();
            if (classes.Count == 0 || !classes.Any() || classes == null)
            {
                throw new CustomError(404,"Not Found","Please create Class First");
            }
            Gene[][] gen = new Gene[7][];
            for (int i = 0; i < 7; i++)
            {
                gen[i] = gAServices.
                        Ga(6,
                        classes,
                        PhongHocs(new string[] { "A", "B" }, so_luong_phong_moi_tang: 10, so_tang: 5));
            }

            List<FormatSchedule> ds = new List<FormatSchedule>();

            ds = FormatSchedule.formatSchedule(gen);

            List<LichHoc> dsLH = new List<LichHoc>();
            foreach (var item in ds)
            {
                foreach (var schedule in item.schedules)
                {
                   dsLH.Add(await sqLServices.AddLichHoc(item.DayOfWeek, schedule));
                }
            }

            return Ok();
        }

        static public List<string> PhongHocs(string[] ten_day, int so_luong_phong_moi_tang, int so_tang)
        {
            List<string> PhongHocs = new List<string>();
            if (ten_day == null || so_luong_phong_moi_tang <= 0 || so_tang <= 0)
            {
                throw new Exception("Can't be found Classroom");
            }
            foreach (string ten in ten_day) //ten_day = B, C, D, E 
            { // B
                for (int i = 1; i <= so_tang; i++)
                { //1
                    for (int j = 1; j <= so_luong_phong_moi_tang; j++)
                    { //1
                        PhongHocs.Add($"{ten}{i}0{j}"); //B101
                    }
                }
            }
            return PhongHocs;
        }
    }
}