using AutoMapper;
using HAS.IdentityServer.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDb;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MongoDB.Driver;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDb.IdentityUser;

namespace HAS.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var testConfig = Configuration["MPY:Version"];
            var hasTestConfig = Configuration["HAS:Version"];

            Environment = env;
        }

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));

            services.AddScoped<ProfileContext>();

            services.AddSingleton<IUserStore<IdentityUser>>(provider =>
            {
                var client = new MongoClient(Configuration["MongoDB:Identity:ConnectionString"]);
                var database = client.GetDatabase(Configuration["MongoDB:Identity:Database:Name"]);
                return UserStore<IdentityUser>.CreateAsync(database).GetAwaiter().GetResult();
            });
            
            services.AddIdentity<IdentityUser>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = true;
            });

            var cert = GenerateSelfSignedServerCert();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<IdentityUser>()
                .AddProfileService<HASIdentityProfileService>()
                .AddSigningCredential(cert)
                .AddValidationKey(cert);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }

        private X509Certificate2 GenerateSelfSignedServerCert()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var certificateSecret = kv.GetSecretAsync(Configuration["HAS:SigningCert:Vault"], Configuration["HAS:SigningCert:CertName"]).GetAwaiter().GetResult();

            var privateKeyBytes = Convert.FromBase64String(certificateSecret.Value);

            return new X509Certificate2(privateKeyBytes, (string)null);
        }

        private X509Certificate2 GenerateSelfSignedServerCert(string certificateName)
        {
            var certPassword = Configuration["MPY:IdentityServer:X509CertToken:Password"];
            if(string.IsNullOrEmpty(certPassword))
            {
                throw new ArgumentNullException("Token Signing Certificate Password needs to be set");
            }
            var dnsName = Configuration["MPY:IdentityServer:X509CertToken:DNSName"];
            if(string.IsNullOrEmpty(dnsName))
            {
                throw new ArgumentNullException("Token Signing DNS Name needs to be set.");
            }

            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName(dnsName);
            sanBuilder.AddDnsName(System.Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={certificateName}");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


                request.CertificateExtensions.Add(
                   new X509EnhancedKeyUsageExtension(
                       new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

                request.CertificateExtensions.Add(sanBuilder.Build());

                //var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(365)));

                var yesterday = DateTime.Today.AddDays(-1).ToUniversalTime();
                var future = DateTime.Today.AddDays(365).ToUniversalTime();

                var certificate = request.CreateSelfSigned(new DateTimeOffset(yesterday), new DateTimeOffset(future));
                certificate.FriendlyName = certificateName;

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, certPassword), certPassword, X509KeyStorageFlags.MachineKeySet);
            }
        }
    }
}
