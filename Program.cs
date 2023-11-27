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

            // получаем строку подключения из файла конфигурации
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddDbContext<MobileOperatorContext>(options => options.UseSqlServer(connection));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 5;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
            })
                .AddEntityFrameworkStores<MobileOperatorContext>();

            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ServicePrividedService>();
            builder.Services.AddScoped<SubscriberService>();
            builder.Services.AddScoped<TariffPlanService>();
            builder.Services.AddScoped<TreatyService>();
            builder.Services.AddScoped<TypeTariffService>();

            // добавление кэширования
            builder.Services.AddMemoryCache();

            //Добавление сессий
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

            //Отключение конечных точек
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

            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();     // авторизация

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
