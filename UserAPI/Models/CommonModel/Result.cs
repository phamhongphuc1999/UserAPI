// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System.Net;

namespace UserAPI.Models.CommonModel
{
    public class Result
    {
        public int status { get; set; }
        public object data { get; set; }
    }

    public class Result<TResult>
    {
        public int status { get; set; }
        public TResult data { get; set; }
    }
}
