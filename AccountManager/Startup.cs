using AngularEshop.Data.Contracts;
using AngularEshop.Data.Repositories;
using AutoMapper;
using common.Utilities;
using Common;
using Common.Utilities;
using Data;
using Entities;
using Entities.Role;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Helpers;
using Services.Interfaces;
using Services.Services;
using WebFramework.Configurations;

namespace AccountManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly SiteSettings _siteSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(Configuration["ConnectionStrings:Development"]));

            services.AddCustomIdentity(_siteSettings.IdentitySettings);

            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                    s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddJwtAuthentication(_siteSettings.JwtSettings);

            services.AddRazorPages();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ApplicationIdentityErrorDescriber>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISmsSender, SmsSender>();

            #region CORS

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCors", builder =>
                {
                    builder.WithOrigins("http://example.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        //.AllowCredentials()
                        .Build();
                });
            });

            #endregion CORS
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseCustomExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors("EnableCors");
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}