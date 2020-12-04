﻿// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

namespace UserAPI.Models.CommonModel
{
    public class Result
    {
        public int status { get; set; }
        public object data { get; set; }
    }
}