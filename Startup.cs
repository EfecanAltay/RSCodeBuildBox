using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RSCodeBuildBox.Models;
using RSCodeBuildBox.Service;
using System;
using System.IO;

namespace RSCodeBuildBox
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
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
            var hook_config = Configuration.GetSection(nameof(RSHookConfig)).Get<RSHookConfig>();
            Utils.Utilities.Temp_Repo_Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RSCodeBuildBox", "temp");
            Utils.Utilities.Copy_Path = hook_config.Copy_Path; 

            services.AddControllers();
            services.AddSingleton<IRSGitService, RSGitService>((s0) => { return new RSGitService(hook_config.GithubConfig); });
            services.AddSingleton<IRSDotnetService, RSDotnetService>();
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
