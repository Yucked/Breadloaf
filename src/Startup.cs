using System;
using Breadloaf.Controllers;
using Breadloaf.Infos;
using Breadloaf.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Breadloaf {
    public readonly struct Startup {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddResponseCompression();
            services.AddSingleton<Blockchain>();
            services.AddSingleton<WebSocketController>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseWebSockets(new WebSocketOptions {
                KeepAliveInterval = TimeSpan.FromSeconds(5),
                ReceiveBufferSize = 256
            });

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseMiddleware(typeof(WebSocketMiddleware));

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}