using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

namespace WorkoutTracker
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<PgsqlDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Pgsql")));

            // Подключение моих сервисов и интерфейсов
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
			builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddScoped<IExerciseService, ExerciseService>();
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IWorkoutService, WorkoutService>();
			

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					//options.LoginPath = "/Account/Login";
					//options.AccessDeniedPath = "/Account/AccessDenied";
					options.LoginPath = "/Authorization/Authorization";
					options.AccessDeniedPath = "/Authorization/Authorization";
				});


            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Authorization}/{action=Authorization}/{id?}");

			app.Run();
		}
	}
}
