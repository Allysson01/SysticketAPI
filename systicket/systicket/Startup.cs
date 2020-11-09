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

        // Este m�todo � chamado pelo tempo de execu��o. Use este m�todo para adicionar servi�os ao cont�iner.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("getSysticket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("GET").AllowAnyHeader();
                    });

                options.AddPolicy("postSysticket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("POST").AllowAnyHeader();
                    });

                options.AddPolicy("changeTicket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("PUT").AllowAnyHeader();
                    });

                options.AddPolicy("deleteTicket",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").WithMethods("DELETE").AllowAnyHeader();
                    });
            });

            services.AddControllers();
        }

        // Este m�todo � chamado pelo tempo de execu��o. Use este m�todo para configurar o pipeline de solicita��o HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

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
