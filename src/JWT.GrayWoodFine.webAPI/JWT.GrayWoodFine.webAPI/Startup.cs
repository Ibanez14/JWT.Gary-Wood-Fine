using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWT.GrayWoodFine.webAPI.Models;
using JWT.GrayWoodFine.webAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWT.GrayWoodFine.webAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors();


            #region Step1 (configure token in appsettings/get options as token variable)
            services.Configure<TokenManagement>(Configuration.GetSection(nameof(TokenManagement)));

            var token = Configuration.GetSection(nameof(TokenManagement))
                                     .Get<TokenManagement>();

            #endregion


            #region Step 2 (add authentication configuring default schemas)

            AuthenticationBuilder authBuilder =
            services.AddAuthentication(ops =>
            {
                ops.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            #endregion


            #region Step 3 (Create Token Validation Parameters)

            // Validation Parameters
            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                ValidAudience = token.Audience,
                ValidIssuer = token.Issuer
            };

            #endregion


            #region Step 4 (Add JWT Bearer options)

            authBuilder.AddJwtBearer(ops =>
                {
                    ops.RequireHttpsMetadata = false;
                    ops.SaveToken = true;
                    ops.TokenValidationParameters = tokenValidationParams;
                });

            #endregion


            services.AddScoped<IAuthenticateService, TokenAuthenticateService>();
            services.AddScoped<IUserMangementService, UserMangementService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Step 5 (Add using of cors and authentication)
            app.UseCors(ops => ops.AllowAnyOrigin()
                                      .AllowAnyMethod());

            app.UseAuthentication();

            #endregion

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
