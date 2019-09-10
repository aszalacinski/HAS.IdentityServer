using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDb.IdentityUser;

namespace HAS.IdentityServer
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CloudSettings>(Configuration.GetSection("CloudSettings"));
            services.AddSingleton<IUserStore<IdentityUser>>(provider =>
            {
                var options = provider.GetService<IOptions<CloudSettings>>();
                var client = new MongoClient(options.Value.DBConnectionString_MongoDB);
                var database = client.GetDatabase(options.Value.DBConnectionString_MongoDB_DatabaseName);

                return UserStore<IdentityUser>.CreateAsync(database).GetAwaiter().GetResult();
            });

            services.AddIdentity<IdentityUser>();

            // uncomment, if you want to add an MVC-based UI
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                //.AddTestUsers(Config.GetUsers());
                .AddAspNetIdentity<IdentityUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            // uncomment if you want to support static files
            app.UseStaticFiles();


            // uncomment, if you want to add an MVC-based UI
            app.UseMvcWithDefaultRoute();
        }
    }
}
