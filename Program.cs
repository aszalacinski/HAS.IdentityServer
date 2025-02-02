﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;

namespace HAS.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "IdentityServer4";

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((ctx, builder) =>
                    {
                        builder.SetBasePath(Directory.GetCurrentDirectory())
                            .AddEnvironmentVariables();

                        var config = builder.Build();

                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        builder.AddAzureKeyVault(
                            $"https://{config["Azure_KeyVault_MPY_Vault"]}.vault.azure.net/",
                            keyVaultClient,
                            new DefaultKeyVaultSecretManager()
                            );

                        builder.AddAzureKeyVault(
                            $"https://{config["Azure_KeyVault_HAS_Vault"]}.vault.azure.net/",
                            keyVaultClient,
                            new DefaultKeyVaultSecretManager()
                            );

                        if (ctx.HostingEnvironment.IsDevelopment())
                        {
                            builder.AddUserSecrets<Startup>();
                        }                        
                    })
                    .UseStartup<Startup>()
                    .UseSerilog((context, configuration) =>
                    {
                        configuration
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.File(@"identityserver4_log.txt")
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                    });
        }
    }
}