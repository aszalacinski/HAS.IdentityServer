// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
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

        public static IEnumerable<Client> GetClients()
        {

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

                // OpenId Connect Implicit Flow client sample - use for MVC style
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