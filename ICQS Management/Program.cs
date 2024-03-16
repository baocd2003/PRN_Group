
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository;

namespace ICQS_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddMvc().AddRazorPagesOptions(option => option.Conventions.AddPageRoute("/HomePage", ""));
            builder.Services.AddSession(o => o.IdleTimeout = TimeSpan.FromMinutes(5));
            builder.Services.AddDbContext<applicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddHttpContextAccessor();
            //Scope
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IBatchManagement, BatchManagementRepository>();
            builder.Services.AddScoped<IMaterialManagementRepository, MaterialManagementRepository>();
            builder.Services.AddScoped<IMaterialTypeManagementRepository, MaterialTypeManagementRepository>();
            builder.Services.AddScoped<IQuotationManagementRepository, QuotationManagementRepository>();
            builder.Services.AddScoped<IProjectManagementRepository, ProjectManagementRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}