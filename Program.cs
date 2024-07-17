using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Note.Data;
using Note.Models;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>( options =>
    //sử dụng để lấy chuỗi kết nối từ file cấu hình (appsettings.json)
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//khởi tạo dịch vụ identity (đk UserManager, RoleManager)
builder.Services.AddIdentity<Users, Roles>(options => {
    options.Password.RequireDigit = true; //mk chứa ít nhất 1 số
    options.Password.RequireLowercase = true; //mk chứa ít nhất 1 chữ cái viết thường
    options.Password.RequireUppercase = true; // mk chứa ít nhất 1 chữ cái viết hoa
    options.Password.RequireNonAlphanumeric = true; // mk chứa ít nhất 1 ký tự ko phải chữ và số
    options.Password.RequiredLength = 8; // độ dài tối thiểu của mk là  8
}).AddEntityFrameworkStores<ApplicationDbContext>(); // cung cấp cơ sở dữ liệu cho dịch vụ Identity, sử dụng Entity Framework Core

//Cấu hình JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options => {

        var signingKey = builder.Configuration["JWT:SigningKey"];
        if (signingKey == null) {
          throw new ArgumentNullException("Failed");
        }

        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(signingKey)
            )
        };
    }
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.Run();
