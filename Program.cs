using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SotovayaSvyas.Data;
using SotovayaSvyas.Services;

namespace SotovayaSvyas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ������ ����������� �� ����� ������������
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddDbContext<MobileOperatorContext>(options => options.UseSqlServer(connection));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 5;   // ����������� �����
                opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                opts.Password.RequireDigit = false; // ��������� �� �����
            })
                .AddEntityFrameworkStores<MobileOperatorContext>();

            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ServicePrividedService>();
            builder.Services.AddScoped<SubscriberService>();
            builder.Services.AddScoped<TariffPlanService>();
            builder.Services.AddScoped<TreatyService>();
            builder.Services.AddScoped<TypeTariffService>();

            // ���������� �����������
            builder.Services.AddMemoryCache();

            //���������� ������
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".RoomRental.Session";
                //options.IdleTimeout = System.TimeSpan.FromSeconds(2*10+240);
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddMvc();

            //���������� �������� �����
            builder.Services.AddControllersWithViews(mvcOptions =>
            {
                mvcOptions.EnableEndpointRouting = false;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();    // ��������������
            app.UseAuthorization();     // �����������

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}
