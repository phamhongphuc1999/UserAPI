// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class CategoryService: BaseService<Category>
    {
        public CategoryService(string database, string collection): base(database)
        {

        }
    }
}
