using System.Security.Cryptography;
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.MidWare.JWT;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.CheckFace;
using QuanLySinhVien.Service.GGService;
using QuanLySinhVien.Service.HashPassword;

using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.Service.SQL.Iventory;
using QuanLySinhVien.Service.SQL.NguyenLieu;
using QuanLySinhVien.Service.SQL.Order;
using QuanLySinhVien.Service.SQL.ProductS;
using QuanLySinhVien.Service.SQL.Store;
using QuanLySinhVien.Service.SQL.StaffF;
using Serilog;
using QuanLySinhVien.Service.SQL.PhieuNhapKho;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/system_log.txt", //Tạo ra file log 

                             rollingInterval: RollingInterval.Day,//file log được tạo ra hằng ngày 
                             retainedFileCountLimit: 7) // Tự động xóa sau 7 ngày 
            .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobaleFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TokenService>();
// Secret key (lưu trong appsettings.json hoặc biến môi trường)
var secretKey = Env.GetString("JWT_secret")!;
using var sha = SHA256.Create();
var keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
var securityKey = new SymmetricSecurityKey(keyBytes);
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];


// Thêm Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = securityKey
    };
});

builder.Services.AddAuthorization();

var Host = Environment.GetEnvironmentVariable("App_host") ?? "127.0.0.1";
var port = int.Parse(Environment.GetEnvironmentVariable("App_port") ?? "8080");
var SecurityPort = int.Parse(Environment.GetEnvironmentVariable("App_port_https") ?? "443");


//Cấu hình server 
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Parse(Host), port);
    options.Listen(System.Net.IPAddress.Parse(Host), SecurityPort, listenOptions =>
        {
            listenOptions.UseHttps("cert.pfx", Environment.GetEnvironmentVariable("cert_password"));
        });
});

//Cấu hình dịch vụ CORS
builder.Services.AddCors(option =>
    {
        //Cấu hình cho phép mọi nguồn có thể gọi đến API 
        option.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    }
);

//Cấu hình kết nối MS SQL server 

var connectionString = Env.GetString("constr");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));


//Add các service tự viết của project
int workFactor = int.Parse(Environment.GetEnvironmentVariable("WorkFactor") ?? "9");
builder.Services.AddScoped<IPassWordService>(
    sp => new BCryptPasswordService(workFactor)
);


builder.Services.AddScoped<IcheckFace, CheckFace>();
builder.Services.AddScoped<ISheetService, SheetService>();
builder.Services.AddScoped<ISqlStaffServices, SqlStaffServices>();
builder.Services.AddScoped<ISqlStoreServices, SqlStoreServices>();

builder.Services.AddScoped<ISqlProductServiecs, SqlProductServiecs>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<ISqlNguyenLieuServices, SqlNguyenLieuServices>();

builder.Services.AddScoped<ISQLInventoryService,SQLInventoryServices>();

builder.Services.AddScoped<ISqlPhieuNhapKho, SqlPhieuNhapKhoServices>();

var app = builder.Build();

var logger = app.Logger;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuanLySinhVien API V1");
        c.RoutePrefix = string.Empty; // Mở trực tiếp ở root URL: http://localhost:5000/
    });
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
// app.UseHttpsRedirection();
app.UseCors();
//Cấu hình kích hoạt các tệp tĩnh 
//Dùng để sử dụng các tẹp tĩnh trong folder wwwroot
//Đặt sau app.UseCors để các tệp tĩnh có chính sách Cors
//Đật sau cors và trước UseAuthorization
app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<GlabalLogger>();
app.UseAuthentication();

//Cấu hình các đường dẫn API theo quyền
app.MapGroup("/admin").RequireAuthorization(policy => policy.RequireRole("Admin"));
app.MapGroup("/manager").RequireAuthorization(policy => policy.RequireRole("Manager"));
app.MapGroup("/cashier").RequireAuthorization(policy => policy.RequireRole("Cashier"));

app.UseAuthorization();

app.MapControllers();
logger.LogInformation("Server start at {Time}",DateTime.Now.ToString());
app.Run();
