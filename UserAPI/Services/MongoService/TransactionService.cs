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
    public class TransactionService: BaseService<Transaction>
    {
        private WalletService walletService;
        public TransactionService(string database, string collection): base(database)
        {
            mCollection = mDatabase.GetCollection<Transaction>(collection);
            walletService = new WalletService("MoneyLover", "Wallet");
        }

        public Result InsertTransaction(string walletId, NewTransactionInfo newTransaction)
        {
            Result result = walletService.GetWalletById(walletId);
            if (result.status != 200) return result;
            Transaction transaction = new Transaction()
            {
                walletId = new MongoDBRef("Wallet", ObjectId.Parse(walletId)),
                expenseId = new MongoDBRef("Expense", ObjectId.Parse(newTransaction.expenseId)),
                amount = newTransaction.amount,
                date = newTransaction.date,
                note = newTransaction.note
            };
            mCollection.InsertOne(transaction);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertTransactionAsync(string walletId, NewTransactionInfo newTransaction)
        {
            Result result = await walletService.GetWalletByIdAsync(walletId);
            if (result.status != 200) return result;
            Transaction transaction = new Transaction()
            {
                walletId = new MongoDBRef("Wallet", ObjectId.Parse(walletId)),
                expenseId = new MongoDBRef("Expense", ObjectId.Parse(newTransaction.expenseId)),
                amount = newTransaction.amount,
                date = newTransaction.date,
                note = newTransaction.note
            };
            await mCollection.InsertOneAsync(transaction);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public Result GetTransactionById(string transactionId)
        {
            List<Transaction> result = mCollection.Find(x => x._id == transactionId).ToList();
            Transaction transaction = result.FirstOrDefault();
            if (transaction == null) return new Result
            {
                status = 400,
                data = $"the transaction with id: {transactionId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = transaction
            };
        }

        public async Task<Result> GetTransactionByIdAsync(string transactionId)
        {
            List<Transaction> result = await mCollection.Find(x => x._id == transactionId).ToListAsync();
            Transaction transaction = result.FirstOrDefault();
            if (transaction == null) return new Result
            {
                status = 400,
                data = $"the transaction with id: {transactionId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = transaction
            };
        }

        public Result GetTransactionsByWallet(string walletId)
        {
            List<Transaction> transactions = mCollection.Find(x => x.walletId.Id == BsonValue.Create(walletId)).ToList();
            return new Result
            {
                status = 200,
                data = transactions
            };
        }

        public async Task<Result> GetTransactionsByWalletAsync(string walletId)
        {
            List<Transaction> transactions = await mCollection.Find(x => x.walletId.Id == BsonValue.Create(walletId)).ToListAsync();
            return new Result
            {
                status = 200,
                data = transactions
            };
        }
    }
}
