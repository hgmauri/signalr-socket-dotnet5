using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.Socket.Server.Abstraction;

namespace SignalR.Socket.Server
{
    public class Startup
    {
        const string CORS_SIGNALR_POLICY_NAME = "signalr";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromMinutes(20);
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(40);
                options.HandshakeTimeout = TimeSpan.FromMinutes(5);
                options.MaximumParallelInvocationsPerClient = 10;
                options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
                options.StreamBufferCapacity = 50;
                options.EnableDetailedErrors = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CORS_SIGNALR_POLICY_NAME, builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials()
                );

            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(CORS_SIGNALR_POLICY_NAME);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Socket no ar!");
                });
                endpoints.MapHub<SocketHub>("/sockethub");
                endpoints.MapControllers();
            });
        }
    }
}
