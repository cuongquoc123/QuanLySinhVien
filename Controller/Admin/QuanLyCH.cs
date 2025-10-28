using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.Service.SQL.Store;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("admin/Store")]
    public class QuanLyCH : ControllerBase
    {
        private readonly ISqlStoreServices sqLServices;
        private readonly MyDbContext myDbContext;
        public QuanLyCH(ISqlStoreServices sqLServices, MyDbContext myDbContext)
        {
            this.sqLServices = sqLServices;
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var DSCH = await myDbContext.Stores.ToListAsync();
            if (DSCH.Count == 0 || !DSCH.Any() || DSCH == null)
            {
                throw new KeyNotFoundException("Do not have any store");
            }
            List<Item<Store>> respone = new List<Item<Store>>();
            foreach (var CH in DSCH)
            {
                Item<Store> item = new Item<Store>();
                item.Value = CH;
                item.PathChiTiet = $"/admin/Store/{CH.StoreId}";
                respone.Add(item);
            }
            return Ok(respone);
        }
        [HttpGet("{StoreId}")]
        public async Task<IActionResult> GetCHDetail([FromRoute] string StoreId)
        {
            if (string.IsNullOrEmpty(StoreId))
            {
                throw new ArgumentNullException("Missing Param StoreId");
            }

            var respone = await myDbContext.Stores.FindAsync(StoreId);
            if (respone == null)
            {
                throw new KeyNotFoundException("Store not Exists");
            }

            return Ok(respone);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStoreRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.DiaChi)
                || string.IsNullOrEmpty(request.SDT) || string.IsNullOrEmpty(request.TenCh))
            {
                throw new ArgumentException("This request need 3 Atribute: SDT, TenCh, DiaChi");
            }
            if (request.DiaChi.Length > 50 || request.SDT.Length > 11 || request.TenCh.Length > 50)
            {
                throw new ArgumentOutOfRangeException("Length of atribute is out off range");
            }
            Store newStore = new Store()
            {
                StoreName = request.TenCh,
                StoreAddr = request.DiaChi,
                PhoneNum = request.SDT
            };
            try
            {
                var respone = await sqLServices.CreateStore(newStore);
                if (respone == null)
                {
                    throw new Exception("Fail to Create Store In Database");
                }
                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPut("{storeId}")]
        public async Task<IActionResult> DeteleStore([FromRoute] string storeId)
        {
            if (String.IsNullOrEmpty(storeId))
            {
                throw new ArgumentNullException("Missing Param storeId");
            }
            try
            {
                int Status = await sqLServices.SoftDeleteStore(storeId);
                if (Status == 500)
                {
                    throw new Exception("Can't be soft delete");
                }
                if (Status == 404)
                {
                    throw new KeyNotFoundException("Store not Exists in Database");
                }
                return Ok(new
                {
                    message = "Soft Delete Succesfully"
                });
            } 
            catch(System.Exception)
            {
                throw;
            }
        }
        
    }
}