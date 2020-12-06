// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class CategoryService: BaseService<Category>
    {
        public CategoryService(string database, string collection): base(database)
        {
            mCollection = mDatabase.GetCollection<Category>(collection);
        }

        public Result InsertCategory(string name)
        {
            List<Category> categories = mCollection.Find(x => x.name == name).ToList();
            Category category = categories.FirstOrDefault();
            if (category == null) return new Result
            {
                status = 400,
                data = $"The category with name: ${name} already exist"
            };
            mCollection.InsertOne(new Category { name = name });
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertCategoryAsync(string name)
        {
            List<Category> categories = await mCollection.Find(x => x.name == name).ToListAsync();
            Category category = categories.FirstOrDefault();
            if (category == null) return new Result
            {
                status = 400,
                data = $"The category with name: ${name} already exist"
            };
            mCollection.InsertOne(new Category { name = name });
            return new Result
            {
                status = 200,
                data = ""
            };
        }
    }
}
