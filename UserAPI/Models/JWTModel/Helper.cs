// -------------------- SIMPLE API -------------------- 
//
//
// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
//
//
// Product by: Pham Hong Phuc
//
//
// ----------------------------------------------------

using Microsoft.Extensions.Options;
using System.Security.Claims;
using UserAPI.Configuration;

namespace UserAPI.Models.JWTModel
{
    public static class Helper
    {
        public static JWTContainerModel GetJWTContainerModel(string userId, string username, string email, IOptions<JWTConfig> config)
        {
            return new JWTContainerModel(config)
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, username)
                }
            };
        }

        public static JWTContainerModel GetJWTContainerModel1(string userId, string username, IOptions<JWTConfig> config)
        {
            return new JWTContainerModel(config)
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }
            };
        }
    }
}
