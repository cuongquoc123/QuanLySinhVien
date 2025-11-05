using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using GeneticSharp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.Service.SQL
{
    public abstract class SqLServiceBase 
    {
        protected readonly MyDbContext context;
        protected readonly ILogger logger;
        protected string GenerateId(int so_luong_chu, string KyTuBatDau)
        {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(KyTuBatDau))
            {
                id += KyTuBatDau;
            }
            for (int i = 0; i < so_luong_chu - KyTuBatDau.Length; i++)
            {
                Random ran = new Random();
                int chuKeTiep1 = ran.Next(65, 91);
                int chuKeTiep2 = ran.Next(97, 123);
                int luaChon = ran.Next(0, 2);
                if (luaChon == 1)
                {
                    id += (char)chuKeTiep1;
                }
                else
                {
                    id += (char)chuKeTiep2;
                }
            }
            return id;

        }
        public SqLServiceBase(MyDbContext context, ILoggerFactory logger)
        {
            this.context = context;
            this.logger = logger.CreateLogger("SqLServiceBase");

        }
        
        protected DataTable? TaoBangThamSoSanPham(List<ProductItem> dsP)
        {
            if (dsP.Count == 0 || !dsP.Any() || dsP == null)
            {
                return null;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Ma", typeof(String));
            dt.Columns.Add("SoLuong", typeof(int));

            foreach (ProductItem product in dsP)
            {
                dt.Rows.Add(product.Masp, product.SoLuong);
            }
            return dt;
        }
        
        


    }
}