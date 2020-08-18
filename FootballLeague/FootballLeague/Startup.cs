using FootballLeague.Bll;
using FootballLeague.Bll.Interfaces;
using FootballLeague.Dal;
using FootballLeague.Dal.Interfaces;
using FootballLeague.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballLeague
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
            services.AddDbContext<FootballLeagueDbContext>(options =>
                                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionHristo")));
            services.AddControllers();

            // Add application services.
            services.AddScoped<ITeamEngine, TeamEngine>();
            services.AddScoped<ILeagueEngine, LeagueEngine>();
            services.AddScoped<ILeagueStore, LeagueStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
