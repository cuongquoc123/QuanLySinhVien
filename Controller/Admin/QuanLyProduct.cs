using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.ImgServices;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.Service.SQL.ProductS;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("admin/Product")]
    public class QuanLyProduct : ControllerBase
    {
        private readonly ISqlProductServiecs sqLServices;
        private readonly IImgService imgService;
        public QuanLyProduct(ISqlProductServiecs sqLServices,  IImgService imgService)
        {
            this.sqLServices = sqLServices;
            this.imgService = imgService;  
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest sanphams)
        {
            if (sanphams == null || string.IsNullOrEmpty(sanphams.productname) || string.IsNullOrEmpty(sanphams.mota) || string.IsNullOrEmpty(sanphams.DMID))
            {
                throw new ArgumentException("Missing Product ");
            }
            if (sanphams.file == null || sanphams.file.Length == 0)
            {
                throw new ArgumentException("Missing File Product IMG");
            }
            try
            {
                string relativeFilePath = await imgService.SaveImgIntoProject(sanphams.file);
                Product sp = new Product()
                {
                    ProductName = sanphams.productname,
                    Price = sanphams.donGia,
                    SubcategoryId = sanphams.DMID,
                    Decription = sanphams.mota
                };
                var respone = await sqLServices.CreateProDucts(sp, relativeFilePath);

                if (respone == null)
                {
                    throw new Exception($"Failed to create products with img");
                }
                return Ok(new
                {
                    message = "Create Successfull",
                    data = respone
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpPut("SoftDelete/{ProductId}")]
        public async Task<IActionResult> DeleteSoft([FromRoute] string ProductId)
        {
            try
            {
                if (string.IsNullOrEmpty(ProductId))
                {
                    throw new ArgumentException("Missing Param ProductId");
                }
                int Status = await sqLServices.SoftDeleteProduct(ProductId);
                if (Status == 404)
                {
                    throw new KeyNotFoundException("Product Not Found");
                }
                if (Status == 500)
                {
                    throw new Exception("Can't soft Delete this Product");
                }
                else
                {
                    return Ok(new
                    {
                        message = "Successfully Soft Delete ProductId"
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}