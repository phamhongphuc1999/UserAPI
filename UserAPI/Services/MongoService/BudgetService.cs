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
using static UserAPI.Program;

namespace UserAPI.Services.MongoService
{
    public class BudgetService: BaseService<Budget>
    {
        public BudgetService(string collection): base(collection)
        {
        }

        public Result InsertBudget(string walletId, string username, NewBudgetInfo newBudget)
        {
            Result result = walletService.GetWalletById(walletId, username);
            if (result.status != 200) return result;
            Budget budget = new Budget
            {
                amount = newBudget.amount,
                walletId = walletId,
                categoryId = newBudget.categoryId,
                dateFrom = Helper.ConvertStringToTime(newBudget.dateFrom),
                dateTo = Helper.ConvertStringToTime(newBudget.dateTo)
            };
            mCollection.InsertOne(budget);
            return new Result
            {
                status = 200,
                data = budget
            };
        }

        public async Task<Result> InsertBudgetAsync(string walletId, string username, NewBudgetInfo newBudget)
        {
            Result result = await walletService.GetWalletByIdAsync(walletId, username);
            if (result.status != 200) return result;
            Budget budget = new Budget
            {
                amount = newBudget.amount,
                walletId = walletId,
                categoryId = newBudget.categoryId,
                dateFrom = Helper.ConvertStringToTime(newBudget.dateFrom),
                dateTo = Helper.ConvertStringToTime(newBudget.dateTo)
            };
            mCollection.InsertOne(budget);
            return new Result
            {
                status = 200,
                data = budget
            };
        }

        public Result GetBudgetById(string budgetId)
        {
            Budget budget = mCollection.Find(x => x._id == budgetId).ToList().FirstOrDefault();
            if (budget == null) return new Result
            {
                status = 400,
                data = $"budget with id: {budgetId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = budget
            };
        }

        public async Task<Result> GetBudgetByIdAsync(string budgetId)
        {
            List<Budget> budgets = await mCollection.Find(x => x._id == budgetId).ToListAsync();
            Budget budget = budgets.FirstOrDefault();
            if (budget == null) return new Result
            {
                status = 400,
                data = $"budget with id: {budgetId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = budget
            };
        }

        public Result GetBudgetsByWallet(string walletId, string[] categoryIdList = null)
        {
            List<Budget> budgets = new List<Budget>();
            if (categoryIdList != null)
            {
                budgets = mCollection.Find(x =>
                    x.walletId == walletId && categoryIdList.Contains(x.categoryId)
                 ).ToList();
            }
            else budgets = mCollection.Find(x => x.walletId == walletId).ToList();
            return new Result
            {
                status = 200,
                data = budgets
            };
        }

        public async Task<Result> GetBudgetsByWalletAsync(string walletId, string[] categoryIdList = null)
        {
            List<Budget> budgets = new List<Budget>();
            if (categoryIdList != null)
            {
                budgets = await mCollection.Find(x =>
                    x.walletId == walletId && categoryIdList.Contains(x.categoryId)
                 ).ToListAsync();
            }
            else budgets = await mCollection.Find(x => x.walletId == walletId).ToListAsync();
            return new Result
            {
                status = 200,
                data = budgets
            };
        }
    }
}
