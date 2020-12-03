// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System.Security.Claims;

namespace UserAPI.Models.JWTModel
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; }
        Claim[] Claims { get; set; }
    }
}
