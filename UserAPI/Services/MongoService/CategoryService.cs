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

        public Result InsertCategory(NewCategoryInfo newCategory)
        {
            Category category = mCollection.Find(x => x.name == newCategory.name).FirstOrDefault();
            if (category != null) return new Result
            {
                status = 400,
                data = $"The category with name: {newCategory.name} already exist"
            };
            mCollection.InsertOne(new Category
            {
                name = newCategory.name,
                iconId = newCategory.iconId
            });
            category = mCollection.Find(x => x.name == newCategory.name).FirstOrDefault();
            return new Result
            {
                status = 200,
                data = category
            };
        }

        public async Task<Result> InsertCategoryAsync(NewCategoryInfo newCategory)
        {
            Category category = await mCollection.Find(x => x.name == newCategory.name).FirstOrDefaultAsync();
            if (category != null) return new Result
            {
                status = 400,
                data = $"The category with name: {newCategory.name} already exist"
            };
            mCollection.InsertOne(new Category { 
                name = newCategory.name,
                iconId = newCategory.iconId
            });
            category = await mCollection.Find(x => x.name == newCategory.name).FirstOrDefaultAsync();
            return new Result
            {
                status = 200,
                data = category
            };
        }

        public Result GetCategoryById(string categoryId)
        {
            Category category = mCollection.Find(x => x._id == categoryId).ToList().FirstOrDefault();
            if (category == null) return new Result
            {
                status = 400,
                data = $"the category with id: {categoryId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = category
            };
        }

        public async Task<Result> GetCategoryByIdAsync(string categoryId)
        {
            List<Category> categorys = await mCollection.Find(x => x._id == categoryId).ToListAsync();
            Category category = categorys.FirstOrDefault();
            if (category == null) return new Result
            {
                status = 400,
                data = $"the category with id: {categoryId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = category
            };
        }
    }
}
