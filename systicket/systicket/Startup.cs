using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace systicket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo tempo de execução. Use este método para adicionar serviços ao contêiner.
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

            //services.AddControllers();
            services.AddControllersWithViews();
        }

        // Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            //app.UseCors(option => option.AllowAnyOrigin());
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
