using HandyHub.Data;
using HandyHub.Repositories;
using HandyHub.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static HandyHub.Repositories.IRepository;

namespace HandyHub
{
    public class Program
    {
        public static void Main ( string[] args )
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add MVC controllers and views
            builder.Services.AddControllersWithViews();

            // Database connection setup
            builder.Services.AddDbContext<HandyHubDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Read JWT settings from configuration
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var jwtKey = jwtSection["Key"];
            var jwtIssuer = jwtSection["Issuer"];
            var jwtAudience = jwtSection["Audience"];

            // Register JWT service
            builder.Services.AddScoped<JwtService>(provider =>
                new JwtService(jwtKey));

            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["jwt"];
                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

            // Register repositories and business services
            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IService<>), typeof(GenericService<>));
            builder.Services.AddScoped(typeof(GenericService<>));
            builder.Services.AddScoped<WorkerService>();
            builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IWorkerService, WorkerService>();    
            builder.Services.AddScoped<IClientService, ClientService>();


            var app = builder.Build();

            // Error page setup (Development or Production)
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Static files, routing, authentication, authorization
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Default route mapping
            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");



            app.Run();
        }
    }
}
