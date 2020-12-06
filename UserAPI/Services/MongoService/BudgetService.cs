// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class BudgetService: BaseService<Budget>
    {
        public WalletService walletService;

        public BudgetService(string database, string collection) : base(database)
        {
            mCollection = mDatabase.GetCollection<Budget>(collection);
            walletService = new WalletService("MoneyLover", "Wallet");
        }

        public Result InsertBudget(string walletId, NewBudgetInfo newBudget)
        {
            Result result = walletService.GetWalletById(walletId);
            if (result.status != 200) return result;
            Wallet wallet = (Wallet)result.data;
            mCollection.InsertOne(new Budget
            {
                amount = newBudget.amount,
                walletId = new MongoDBRef("Wallet", ObjectId.Parse(wallet._id)),
                categoryId = new MongoDBRef("Category", ObjectId.Parse(newBudget.categoryId)),
                dateFrom = new BsonDateTime(HelperService.ConvertStringToTime(newBudget.dateFrom)),
                dateTo = new BsonDateTime(HelperService.ConvertStringToTime(newBudget.dateTo))
            });
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertBudgetAsync(string walletId, NewBudgetInfo newBudget)
        {
            Result result = await walletService.GetWalletByIdAsync(walletId);
            if (result.status != 200) return result;
            Wallet wallet = (Wallet)result.data;
            mCollection.InsertOne(new Budget
            {
                amount = newBudget.amount,
                walletId = new MongoDBRef("Wallet", ObjectId.Parse(wallet._id)),
                categoryId = new MongoDBRef("Category", ObjectId.Parse(newBudget.categoryId)),
                dateFrom = new BsonDateTime(HelperService.ConvertStringToTime(newBudget.dateFrom)),
                dateTo = new BsonDateTime(HelperService.ConvertStringToTime(newBudget.dateTo))
            });
            return new Result
            {
                status = 200,
                data = ""
            };
        }
    }
}
