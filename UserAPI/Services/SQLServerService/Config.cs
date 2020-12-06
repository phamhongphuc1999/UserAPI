// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System.Collections.Generic;

namespace UserAPI.Services.SQLServerService
{
    public static class Config
    {
        public static string[] EMPLOYEE_FIELDS = new string[]
        {
            "Username", "Password", "Name", "Image", "Birthday", "Gender", "Phone", "Address", "Position", "Node"
        };
    }
}
