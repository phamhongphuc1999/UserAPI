// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class ExpenseService: BaseService<Expense>
    {
        public ExpenseService(string database, string collection): base(database)
        {
            mCollection = mDatabase.GetCollection<Expense>(collection);
        }

        public Result InsertExpense(string categoryId, NewExpenseInfo newExpense)
        {
            List<Expense> expenses = mCollection.Find(x => x.name == newExpense.name).ToList();
            if (expenses.Count > 0) return new Result
            {
                status = 400,
                data = $"The expense with name: ${newExpense.name} already exist"
            };
            mCollection.InsertOne(new Expense
            {
                name = newExpense.name,
                categoryId = new MongoDBRef("Category", ObjectId.Parse(categoryId)),
                iconId = new MongoDBRef("Icon", ObjectId.Parse(newExpense.iconId))
            });
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertExpenseAsync(string categoryId, NewExpenseInfo newExpense)
        {
            List<Expense> expenses = await mCollection.Find(x => x.name == newExpense.name).ToListAsync();
            if (expenses.Count > 0) return new Result
            {
                status = 400,
                data = $"The expense with name: ${newExpense.name} already exist"
            };
            mCollection.InsertOne(new Expense
            {
                name = newExpense.name,
                categoryId = new MongoDBRef("Category", ObjectId.Parse(categoryId)),
                iconId = new MongoDBRef("Icon", ObjectId.Parse(newExpense.iconId))
            });
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public Result GetExpenseById(string expenseId)
        {
            Expense expense = mCollection.Find(x => x._id == expenseId).ToList().FirstOrDefault();
            if (expense == null) return new Result
            {
                status = 400,
                data = $"The expense with id: ${expenseId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = expense
            };
        }

        public async Task<Result> GetExpenseByIdAsync(string expenseId)
        {
            List<Expense> expenses = await mCollection.Find(x => x._id == expenseId).ToListAsync();
            Expense expense = expenses.FirstOrDefault();
            if (expense == null) return new Result
            {
                status = 400,
                data = $"The expense with id: ${expenseId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = expense
            };
        }

        public Result GetExpensesByCategory(string categoryId)
        {
            List<Expense> expenses = mCollection.Find(x => x.categoryId.Id == BsonValue.Create(categoryId)).ToList();
            return new Result
            {
                status = 200,
                data = expenses
            };
        }

        public async Task<Result> GetExpensesByCategoryAsync(string categoryId)
        {
            List<Expense> expenses = await mCollection.Find(x => x.categoryId.Id == BsonValue.Create(categoryId)).ToListAsync();
            return new Result
            {
                status = 200,
                data = expenses
            };
        }
    }
}
