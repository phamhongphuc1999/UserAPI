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
        public CategoryService(string collection): base(collection)
        {
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

        public Result GetSingerCategoryById(string categoryId)
        {
            Category category = mCollection.Find(x => x._id == categoryId).FirstOrDefault();
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

        public async Task<Result> GetSingerCategoryByIdAsync(string categoryId)
        {
            Category category = await mCollection.Find(x => x._id == categoryId).FirstOrDefaultAsync();
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

        public Result GetCategoryById(string categoryId)
        {
            Category category = mCollection.Find(x => x._id == categoryId).FirstOrDefault();
            if (category == null) return new Result
            {
                status = 400,
                data = $"the category with id: {categoryId} do not exist"
            };
            List<Category> categories = mCollection.Find(x => x.parentCategory == category._id).ToList();
            return new Result
            {
                status = 200,
                data = new
                {
                    category = category,
                    childrent = categories
                }
            };
        }

        public async Task<Result> GetCategoryByIdAsync(string categoryId)
        {
            Category category = await mCollection.Find(x => x._id == categoryId).FirstOrDefaultAsync();
            if (category == null) return new Result
            {
                status = 400,
                data = $"the category with id: {categoryId} do not exist"
            };
            List<Category> categories = await mCollection.Find(x => x.parentCategory == category._id).ToListAsync();
            return new Result
            {
                status = 200,
                data = new
                {
                    category = category,
                    childrent = categories
                }
            };
        }

        public Result GetListCategories()
        {
            List<Category> categories = mCollection.Find(x => x.parentCategory == null).ToList();
            List<object> list = new List<object>();
            foreach (Category category in categories)
            {
                List<Category> childrent = mCollection.Find(x => x.parentCategory == category._id).ToList();
                list.Add(new
                {
                    category = category,
                    childrent = childrent
                });
            }
            return new Result
            {
                status = 200,
                data = list
            };
        }

        public async Task<Result> GetListCategoriesAsync()
        {
            List<Category> categories = await mCollection.Find(x => x.parentCategory == null).ToListAsync();
            List<object> list = new List<object>();
            foreach (Category category in categories)
            {
                List<Category> childrent = await mCollection.Find(x => x.parentCategory == category._id).ToListAsync();
                list.Add(new
                {
                    category = category,
                    childrent = childrent
                });
            }
            return new Result
            {
                status = 200,
                data = list
            };
        }
    }
}
