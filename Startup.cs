using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Converge.API.Data;
using CP.API.Data;
using CP.API.Helpers;
using CP.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SAMMAPP.API.Helpers;

namespace CP.API
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
            services.AddDbContext<DataContext>(p =>p.UseSqlServer(Configuration.GetConnectionString("DefualtConnection")));
            IdentityBuilder builder = services.AddIdentityCore<Supplier>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

            });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<Supplier>>();

             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
            (Options =>
           {
               Options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:token").Value)
                     ),
                   ValidateIssuer = false,
                   ValidateAudience = false


               };

           }
            );
              services.AddAuthorization(
                options=>{
                    options.AddPolicy("RequireAdminRole",policy=>policy.RequireRole("Admin"));
                    options.AddPolicy("ModerateSupplierRole",policy=>policy.RequireRole("Admin","Moderator"));
                    options.AddPolicy("EstablishAStore",policy=>policy.RequireRole("EstablishAStore","Admin"));
                    options.AddPolicy("Products",policy=>policy.RequireRole("Products","Admin"));
                    options.AddPolicy("Deals",policy=>policy.RequireRole("Deals","Admin"));
                    options.AddPolicy("DiscountCoupons",policy=>policy.RequireRole("DiscountCoupons","Admin"));
                    options.AddPolicy("NewOrders",policy=>policy.RequireRole("NewOrders","Admin"));
                    options.AddPolicy("Reports",policy=>policy.RequireRole("Reports","Admin"));
                    options.AddPolicy("TechnicalSupport",policy=>policy.RequireRole("TechnicalSupport","Admin"));
                    options.AddPolicy("VIP",policy=>policy.RequireRole("VIP","Admin"));
                }
            );
          services.AddTransient<TrialData>();
            services.AddMvc(options=>{
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add( new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
             
            .AddJsonOptions(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            });
            services.AddCors();
            services.Configure<CloudinarySetting>(Configuration.GetSection("CloudinarySetting"));
            services.AddAutoMapper();
            //  Mapper.Reset();
            services.AddScoped<ICPRepository, CPRepository>();
            
             Mapper.Reset();

           
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env ,TrialData trialData)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(BuilderExtensions =>
                   {
                       BuilderExtensions.Run(async context =>
                       {
                           context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                           var error = context.Features.Get<IExceptionHandlerFeature>();
                           if (error != null)
                           {
                               context.Response.addApplicationError(error.Error.Message);
                               await context.Response.WriteAsync(error.Error.Message);
                           }
                       });
                   });
                // app.UseHsts();

            }
            trialData.TrialSuppliers();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseDefaultFiles();
            app.UseAuthentication();
            app.Use(async(context,next)=>{
                await next();
                if(context.Response.StatusCode ==404){
                    context.Request.Path="/index.html";
                    await next();
                }
                
                            });
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
