using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace BPCloud_VP.ReportService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration JWTSecurityConfig = Configuration.GetSection("JWTSecurity");
            string securityKey = JWTSecurityConfig.GetValue<string>("securityKey");
            string issuer = JWTSecurityConfig.GetValue<string>("issuer");
            string audience = JWTSecurityConfig.GetValue<string>("audience");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddDbContext<ReportContext>(o => o.UseSqlServer(Configuration.GetConnectionString("ReportContext"), op
                 => op.EnableRetryOnFailure()));
            services.AddTransient<IPaymentReportRepository, PaymentReportRepository>();
            services.AddTransient<IInvoiceReportRepository, InvoiceReportRepository>();

            services.AddTransient<IOverviewRepository, OverviewRepository>();
            services.AddTransient<IPPMRepository, PPMRepository>();
            services.AddTransient<IVendorRatingRepository, VendorRatingRepository>();

            services.AddTransient<IDOLRepository, DOLRepository>();
            services.AddTransient<IFGCPSRepository, FGCPSRepository>();
            services.AddTransient<IGRRRepository, GRRRepository>();
            services.AddTransient<IIPRepository, IPRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
