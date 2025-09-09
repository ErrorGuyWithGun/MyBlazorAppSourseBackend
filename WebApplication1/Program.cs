using Microsoft.AspNetCore.Identity;
using WebApplication1.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Web.Email.Model;
using Web.Email.Services;
using WebApplication1.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = configuration["JWT:ValidIssuer"],
             ValidAudience = configuration["JWT:ValidAudience"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
         };
     }).AddFacebook(opt =>
     {
         opt.AppId = configuration["Facebook:AppId"]!;
         opt.AppSecret = configuration["Facebook:AppSecret"]!;
         }
    );

builder.Services.Configure<IdentityOptions>(
    options => options.SignIn.RequireConfirmedEmail = true);

var emailConfig = configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();


builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=db26079.public.databaseasp.net; 
                        Database=db26079; 
                        User Id=db26079; 
                        Password=Yg3=f#K4N5!y; 
                        Encrypt=True;
                        TrustServerCertificate=True;
                        MultipleActiveResultSets=True;"));

//builder.Services.AddCors(options =>
//    {
//        options.AddPolicy("AllowLocalhost7170", policy =>
//        {
//            policy.WithOrigins(configuration["Cors:Local"]!)
//                  .AllowAnyMethod()
//                  .AllowAnyHeader()
//                  .AllowCredentials();
//        });
//    });
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost7170", policy =>
        {
            policy.WithOrigins(configuration["Cors:Online"]!)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); 

app.UseCors("AllowLocalhost7170");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();