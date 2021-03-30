// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Services.JWTService;

namespace UserAPI.Controllers.MongoControllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ProductController: ControllerBase
    {
        private readonly IOptions<JWTConfig> _jwtConfig;
        private IAuthService authService;

        public ProductController(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
            authService = new JWTService(_jwtConfig.Value.SecretKey);
        }


    }
}
