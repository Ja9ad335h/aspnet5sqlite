using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WhatsappGroups.Business.Services;
using Newtonsoft.Json;
using Microsoft.AspNet.Http;
using System.IdentityModel.Tokens;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Authentication.JwtBearer;
using Microsoft.AspNet.Authorization;
using WhatsappGroups.Business.Core;
using Newtonsoft.Json.Serialization;
using WhatsappGroups.Business.Extensions;
using WhatsappGroups.Connections;
using WhatsappGroups.Business.Providers;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using WhatsappGroups.Business.Models;

namespace WhatsappGroups
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddCaching();

            services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            //services.AddCors(o => o.AddPolicy("AllowAll", p => p.AllowAnyOrigin()));
            services.AddSignalR(options => options.Hubs.EnableDetailedErrors = true);

            //application services.
            services.AddScopedDbContexts();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailSender, AuthMessageSender>();
            services.AddScoped<ISmsSender, AuthMessageSender>();
            services.AddScoped<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Use(async (context, next) =>
                    {
                        var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                        // This should be much more intelligent - at the moment only expired 
                        // security tokens are caught - might be worth checking other possible 
                        // exceptions such as an invalid signature.
                        if (error != null)
                        {
                            if (error.Error is SecurityTokenExpiredException || error.Error is SecurityTokenInvalidSignatureException || error.Error is SecurityTokenInvalidSigningKeyException)
                            {
                                context.Response.StatusCode = 401;
                                // What you choose to return here is up to you, in this case a simple 
                                // bit of JSON to say you're no longer authenticated.
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { authenticated = false, tokenExpired = true, error = error.Error.Message }));
                            }
                            else
                            {
                                context.Response.StatusCode = 500;
                                context.Response.ContentType = "application/json";
                                // TODO: Shouldn't pass the exception message straight out, change this.
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { success = false, error = error.Error?.Message }));
                            }
                        }
                        // We're not trying to handle anything else so just let the default 
                        // handler handle.
                        else await next();
                    });
                });

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        //serviceScope.ServiceProvider.GetService<WhatsappGroupsDataContext>().Database.Migrate();
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler();
            app.UseStaticFiles();

            app.UseJwtBearerAuthentication(options => {
                options.AutomaticAuthenticate = true;
                options.Audience = "resource_server_1";
                options.Authority = "http://localhost:54540/api";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = true;

            });

            // Add a new middleware issuing tokens.
            app.UseOpenIdConnectServer(options => {
                options.AuthenticationScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.Provider = new AuthorizationProvider();

                // Note: see AuthorizationController.cs for more
                // information concerning ApplicationCanDisplayErrors.
                options.ApplicationCanDisplayErrors = true;
                options.AllowInsecureHttp = true;
                options.Issuer = new Uri("http://localhost:54540/");

                // Note: by default, tokens are signed using dynamically-generated
                // we are using our own RSA from database
                //options.SigningCredentials.AddKey(Cryptography.GetRSASecurityKey());
                options.TokenEndpointPath = "/api/connect/token";
                options.AuthorizationEndpointPath = "/api/authorize";
                options.ConfigurationEndpointPath = "/api/.well-known/openid-configuration";
                options.CryptographyEndpointPath = "/api/.well-known/jwks";
                options.LogoutEndpointPath = "/api/connect/logout";
                options.ProfileEndpointPath = "/api/connect/userinfo";
                options.ValidationEndpointPath = "/api/connect/introspect";

            });

            app.UseCors("AllowAll");
            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("api", "api/{controller}/{action}/{id?}", new { action = "Get" });
            });
            app.UseWebSockets();
            app.UseSignalR<ClientConnection>("/signalr");
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
