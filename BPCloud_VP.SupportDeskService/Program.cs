
using BPCloud_VP.SupportDeskService.DBContexts;
using BPCloud_VP.SupportDeskService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Values;
using System.Text;
using System.Xml.Serialization;

namespace BPCloud_VP.SupportDeskService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            IConfiguration JWTSecurityConfig = builder.Configuration.GetSection("JWTSecurity");
            string securityKey = JWTSecurityConfig.GetValue<string>("securityKey");
            string issuer = JWTSecurityConfig.GetValue<string>("issuer");
            string audience = JWTSecurityConfig.GetValue<string>("audience");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //what to validate
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        //setup validate data
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = symmetricSecurityKey
                    };
                });


            // Add services to the container.
            builder.Services.AddCors(options => { options.AddPolicy("cors", a => a.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); });
            var connectionStrings = builder.Configuration.GetConnectionString("SupportDeskContext");
            builder.Services.AddDbContext<SupportDeskContext>(options => options.UseNpgsql(connectionStrings));
            builder.Services.AddTransient<ISupportDeskRepository, SupportDeskRepository>();


            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
