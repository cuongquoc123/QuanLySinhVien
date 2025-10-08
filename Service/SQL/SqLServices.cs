using GeneticSharp;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL
{
    public class SqLService : ISqLServices
    {
        private readonly MyDbContext context;

        private readonly ILogger<SqLService> logger;


        public SqLService(MyDbContext context, ILogger<SqLService> logger)
        {
            this.context = context;
            this.logger = logger;
        }


    }
}