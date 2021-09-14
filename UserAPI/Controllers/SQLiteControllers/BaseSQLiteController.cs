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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Configuration;
using UserAPI.Services.JWTService;

namespace UserAPI.Controllers.SQLiteControllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BaseSQLiteController : ControllerBase
    {
        protected readonly IOptions<JWTConfig> _jwtConfig;
        protected IAuthService authService;

        public BaseSQLiteController(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
            authService = new JWTService(_jwtConfig.Value.SecretKey);
        }
    }
}
