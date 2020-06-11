using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MvcClient
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "MvcCookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("MvcCookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:44382/";
                //options.ClaimActions.DeleteClaim("amr");
                options.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.grandma");
                options.ClientId = "client_id_mvc";
                options.ClientSecret = "client_secret_mvc";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ResponseType = "code";
                options.SaveTokens = true;
                
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("rc.scope");
                options.Scope.Add("ApiOne");
                options.Scope.Add("ApiTwo");
            });

            services.AddHttpClient();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
