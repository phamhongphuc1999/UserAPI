// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

namespace UserAPI.Models.CommonModel
{
    public class ResponseSuccessType
    {
        public string status { get; set; }
        public object data { get; set; }
    }

    public class ResponseFailType
    {
        public string status { get; set; }
        public object reason { get; set; }
    }

    public static class Responder
    {
        public static object Success(object data = null)
        {
            if(data != null) return new ResponseSuccessType
            {
                status = "success",
                data = data
            };
            return new
            {
                status = "success",
                data = "null"
            };
        }

        public static object Fail(object reason)
        {
            return new ResponseFailType
            {
                status = "fail",
                reason = reason
            };
        }
    }
}
