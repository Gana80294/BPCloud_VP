using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace BPCloud_VP_POService
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
            //services.AddDbContext<POContext>(o => o.UseSqlServer(Configuration.GetConnectionString("POContext")));
            services.AddDbContext<POContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("POContext"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                });
            });
            services.AddTransient<IDashboardRepositorie, DashboardRepositorie>();
            services.AddTransient<IAIACTRepository, AIACTRepository>();
            services.AddTransient<IPORepository, PORepository>();
            services.AddTransient<IAttachmentRepository, AttachmentRepository>();
            services.AddTransient<IFLIPCostRepository, FLIPCostRepository>();
            services.AddTransient<IASNRepository, ASNRepository>();
            services.AddTransient<IMasterRepository, MasterRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPurchaseIndentRepository, PurchaseIndentRepository>();
            services.AddTransient<IReturnRepository, ReturnRepository>();
            services.AddTransient<IPODRepository, PODRepository>();
            services.AddTransient<ISubconRepository, SubconRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("MyPolicy");
            app.UseMvc();
        }
    }
}
