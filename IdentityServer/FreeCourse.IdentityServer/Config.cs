﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        //api için token bilgileri
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        //kullanıcı için token bilgileri
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),//email olmak zorunda değil.sadece tokenin erişebilmesi için izin veriyoruz
                       new IdentityResources.OpenId(),//openid olmak zorunda.kullanıcı id
                       new IdentityResources.Profile(),//profile bilgilerine,olmak zorunda değil
                       new IdentityResource()
                       {
                           Name="roles",
                           DisplayName="Roles",
                           Description="Kullanıcı Rolleri",
                           UserClaims = new[]
                           {
                               "role"
                           }
                       }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
                new ApiScope("basket_fullpermission","Basket API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "catalog_fullpermission",
                        "photo_stock_fullpermission",
                        "basket_fullpermission",
                        IdentityServerConstants.LocalApi.ScopeName
                    }
                },
                 new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                         "roles"
                    },
                    AccessTokenLifetime=1*60*60,//1 saatlik
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                     AbsoluteRefreshTokenLifetime = (int)(
                         DateTime.UtcNow.AddDays(60)-DateTime.Now
                     ).TotalSeconds,//60 günlük token.refresh token ömrü
                     RefreshTokenUsage=TokenUsage.ReUse//tekrar kullanalılabilir
                },
                 new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId="WebMvcClientForUserBasketAPI",
                    AllowOfflineAccess=true,
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                          "basket_fullpermission",
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                         "roles"
                    },
                    AccessTokenLifetime=1*60*60,//1 saatlik
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                     AbsoluteRefreshTokenLifetime = (int)(
                         DateTime.UtcNow.AddDays(60)-DateTime.Now
                     ).TotalSeconds,//60 günlük token.refresh token ömrü
                     RefreshTokenUsage=TokenUsage.ReUse//tekrar kullanalılabilir
                }
            };


    }
}