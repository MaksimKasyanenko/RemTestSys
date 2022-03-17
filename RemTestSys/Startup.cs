using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemTestSys;
using RemTestSys.Domain;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.Politics;

namespace RemTestSys
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
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataBase")));
            services.AddTransient<ISessionBuilder, SessionBuilder>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExamAccessService, ExamAccessService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IQuestionService, QuestionService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options => {
                        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Registration");
                        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Editor/Account/LogIn");
                        options.ExpireTimeSpan = AccountValidityPolitics.GetTerm();
                        options.SlidingExpiration = false;
                        }
                );
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Info/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Account}/{action=LogIn}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Student}/{action=AvailableTests}/{id?}");
            });
        }
    }
}
