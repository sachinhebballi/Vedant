using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using SGMH.Healthcare.Vedant.Business;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;
using SGMH.Healthcare.Vedant.Data.Entities;

namespace SGMH.Healthcare.Vedant.API
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
            services.AddDbContext<PatientsContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DoctorConnectionString"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(60));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<PatientsContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = Configuration["Security:Tokens:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = Configuration["Security:Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:Tokens:Key"]))
                };
            });

            services.AddSwaggerGen(swag =>
            {
                swag.SwaggerDoc("v1", new OpenApiInfo { Title = "Vedant API", Version = "1.0" });
                swag.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });
                swag.DocumentFilter<SwaggerSecurityRequirementsDocumentFilter>();
            });

            services.AddScoped(typeof(IAccountLogic), typeof(AccountLogic));
            services.AddScoped(typeof(IPatientLogic), typeof(PatientLogic));
            services.AddScoped(typeof(ICentreLogic), typeof(CentreLogic));
            services.AddScoped(typeof(IConsultationLogic), typeof(ConsultationLogic));
            services.AddScoped(typeof(IDrugLogic), typeof(DrugLogic));
            services.AddScoped(typeof(ITokenLogic), typeof(TokenLogic));
            services.AddScoped(typeof(IReportsLogic), typeof(ReportsLogic));
            services.AddScoped(typeof(IUserContext), typeof(UserContext));
            services.AddHttpContextAccessor();

            services
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opts.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(o =>
            {
                o.AllowAnyHeader();
                o.AllowAnyMethod();
                o.AllowAnyOrigin();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Doctor API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRequestLocalization(options =>
            {
                var ci = new CultureInfo("en-IN");
                options.DefaultRequestCulture = new RequestCulture(ci);
                options.SupportedCultures = new List<CultureInfo>
                {
                    ci
                };
                options.SupportedUICultures = new List<CultureInfo>
                {
                    ci
                };
            });

            app.UseRouting();
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(options => options.MapControllers());
        }
    }
}
