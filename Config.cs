// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HAS.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("MPY.Profile", "HAS MyPractice Profile API"),
                new ApiResource("MPY.Content", "HAS MyPractice Content API")
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {

            var webApp = configuration["MPY:Web:Authority"];
            var demo = configuration["Demo"];

            return new List<Client>
            {
                // Client Credentials (ClientId/Secret) Authentication - used for headless services
                new Client
                {
                    ClientId = "ServiceClientExample",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secrets for authentication
                    ClientSecrets =
                    {
                        new Secret("91E844B6-C96D-41AF-8956-E4801E301D32".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "MPY.Profile", "MPY.Content" }
                },
                // Resource Owner Password Authentication - do not use as it's not as secure
                new Client
                {
                    ClientId = "RO.ClientExample",
                    ClientName = "Resource Owner Example",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("46BC744A-A43C-46AC-A798-2BCE1039A2CD".Sha256())
                    },
                    
                    // scopes that client has access to
                    AllowedScopes = { "MPY.Profile", "MPY.Content" }

                },
                // OpenId Connect Implicit Flow client sample - use for browser based apps - front channel only calling apis directly
                new Client
                {
                    ClientId = "mvcExample",
                    ClientName = "MVC Client Example",
                    AllowedGrantTypes = GrantTypes.Implicit,


                    // the signin and signout callback would be added if you were to use the 
                    // where to redirect to after login
                    RedirectUris = { "http://localhost/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                // OpenId Connect Hybrid Flow client sample - use for server based apps - front channel/back channel - back channel is calling apis directly
                 new Client
                {
                    ClientId = "mvcHybrid",
                    ClientName = "MVC Hybrid Client",

                    // this allows server to server api calls not in a context of a user
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("511536EF-F270-4058-80CA-1C89C192F623".Sha256())
                    },

                    // enable/disable consent with this flag
                    RequireConsent = false,

                    // where to redirect to after login (redirect to calling application)
                    RedirectUris = { "http://<calling app>/signin-oidc" },
                    FrontChannelLogoutUri = "http://<calling app>/signout-oidc",

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://<calling app>/signout-callback-oidc" },

                    AllowedScopes =
                     {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         "MPY.Profile",
                         "MPY.Content"
                     },

                    // permits requesting refresh tokens for long lived API access
                    AllowOfflineAccess = true
                },
                // POSTMAN
                new Client
                {
                    ClientId = "postman-api",
                    ClientName = "Postman Test Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = { "https://getpostman.com/postman" },

                    // NOTE: This link needs to match the link from the presentation layer - oidc-client
                    //          otherwise IdentityServer won't display the link to go back to the site
                    PostLogoutRedirectUris = { "https://www.getpostman.com" },
                    AllowedCorsOrigins = { "https://www.getpostman.com" },
                    EnableLocalLogin = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "MPY.Profile",
                        "MPY.Content"
                    },

                    ClientSecrets =
                    {
                        new Secret("BD39E224-B090-4B21-9CF4-E78F01D6E650".Sha256())
                    },


                },
                // MyPractice.Yoga - Implicit Flow 
                new Client
                {
                    ClientId = "MPY.Web.App",
                    ClientName = "MyPractice.Yoga Content Management App for Instructors",

                    // this allows server to server api calls not in a context of a user
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("4EB30921-C28D-4F61-8D80-5BBE7BAEE6EE".Sha256())
                    },

                    // enable/disable consent with this flag
                    RequireConsent = false,

                    // where to redirect to after login (redirect to calling application)
                    RedirectUris = { $"{configuration["MPY:Web:Authority"]}signin-oidc" },
                    FrontChannelLogoutUri = $"{configuration["MPY:Web:Authority"]}signout-oidc",

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { $"{configuration["MPY:Web:Authority"]}signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MPY.Profile",
                        "MPY.Content"
                    },

                    // permits requesting refresh tokens for long lived API access
                    AllowOfflineAccess = true
                },
                // MyPractive.Yoga - Client Credentials - Registration Event Job
                new Client
                {
                     ClientId = "MPY.RegistrationEvent.Service",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secrets for authentication
                    ClientSecrets =
                    {
                        new Secret("06CCFD72-563C-4EED-9262-7F5637BAFE17".Sha256())
                    },
                    
                    // scopes that client has access to
                    AllowedScopes = { "MPY.Profile" }
                },
                // MyPractice.Yoga - Client Credentials - MPY.Profile Middleware
                // allows for middleware client to talk to Profile API
                new Client
                {
                    ClientId = "MPY.Profile.Middleware",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("F1EF0F7B-E16A-4D1D-844C-2A880D1EB139".Sha256())
                    },

                    AllowedScopes = { "MPY.Profile" }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "F36FD311-622F-4F2A-A595-8325BCB093B4",
                    Username = "aszalacinski@outlook.com",
                    Password = "password"
                }
            };
        }
    }
}