using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace systicket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("getSysticket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("GET").AllowCredentials().AllowAnyHeader();
                    });

                options.AddPolicy("postSysticket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("POST").AllowCredentials().AllowAnyHeader();
                    });

                options.AddPolicy("changeTicket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("PUT").AllowCredentials().AllowAnyHeader();
                    });

                options.AddPolicy("deleteTicket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("DELETE").AllowCredentials().AllowAnyHeader();
                    });
            });

           
            services.AddControllersWithViews();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime=true
                };
            });
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
