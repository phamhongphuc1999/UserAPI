// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

namespace UserAPI.Contances
{
    public static class Messages
    {
        public static readonly string OK = "OK";
        public static readonly string BadRequest = "Bad Request";
        public static readonly string Unauthorized = "Unauthorized";
        public static readonly string Forbidden = "Forbidden";
        public static readonly string WrongUserPassword = "User or password wrong";
        public static readonly string EnableAccount = "This account is enable to login";
        public static readonly string ExistedUser = "This user have existed";
        public static readonly string NotExistedUser = "This user do not existed";
        public static readonly string InvalidToken = "Invalid Token";
    }
}
