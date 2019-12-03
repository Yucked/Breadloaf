using System;
using System.Net;
using Breadloaf.ConnectionHandlers;
using Breadloaf.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Breadloaf {
    public readonly struct Startup {
        public Startup(IConfiguration configuration) { }

        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddWebSockets(x => {
                x.ReceiveBufferSize = 512;
                x.KeepAliveInterval = TimeSpan.FromSeconds(120);
            });

            var blockchain = new Blockchain();
            blockchain.CreateGenesisBlock();
            blockchain.AddNode(new NodeInfo {
                Address = IPEndPoint.Parse("127.0.0.1:5000")
            });
            services.AddSingleton(blockchain);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapConnectionHandler<WebSocketHandler>("/ws");
            });
        }
    }
}