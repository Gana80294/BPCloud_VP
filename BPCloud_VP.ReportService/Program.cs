
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Values;
using System.Text;

namespace BPCloud_VP.ReportService
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
            var connectionStrings = builder.Configuration.GetConnectionString("ReportContext");
            builder.Services.AddDbContext<ReportContext>(options => options.UseNpgsql(connectionStrings));
           
            builder.Services.AddTransient<IPaymentReportRepository, PaymentReportRepository>();
            builder.Services.AddTransient<IInvoiceReportRepository, InvoiceReportRepository>();

            builder.Services.AddTransient<IOverviewRepository, OverviewRepository>();
            builder.Services.AddTransient<IPPMRepository, PPMRepository>();
            builder.Services.AddTransient<IVendorRatingRepository, VendorRatingRepository>();

            builder.Services.AddTransient<IDOLRepository, DOLRepository>();
            builder.Services.AddTransient<IFGCPSRepository, FGCPSRepository>();
            builder.Services.AddTransient<IGRRRepository, GRRRepository>();
            builder.Services.AddTransient<IIPRepository, IPRepository>();

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
            app.UseCors("cors");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
