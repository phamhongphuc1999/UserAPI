// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Driver;
using System;
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
            walletService = new WalletService(database, "Wallet");
        }

        public Result InsertTransaction(string walletId, NewTransactionInfo newTransaction)
        {
            Result result = walletService.GetWalletById(walletId);
            if (result.status != 200) return result;
            Transaction transaction = new Transaction()
            {
                walletId = walletId,
                categoryId = newTransaction.categoryId,
                amount = newTransaction.amount,
                createAt = DateTime.Now,
                note = newTransaction.note
            };
            mCollection.InsertOne(transaction);
            return new Result
            {
                status = 200,
                data = transaction
            };
        }

        public async Task<Result> InsertTransactionAsync(string walletId, NewTransactionInfo newTransaction)
        {
            Result result = await walletService.GetWalletByIdAsync(walletId);
            if (result.status != 200) return result;
            Transaction transaction = new Transaction()
            {
                walletId = walletId,
                categoryId = newTransaction.categoryId,
                amount = newTransaction.amount,
                createAt = DateTime.Now,
                note = newTransaction.note
            };
            await mCollection.InsertOneAsync(transaction);
            return new Result
            {
                status = 200,
                data = transaction
            };
        }

        public Result GetTransactionById(string transactionId)
        {
            Transaction transaction = mCollection.Find(x => x._id == transactionId).FirstOrDefault();
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
            Transaction transaction = await mCollection.Find(x => x._id == transactionId).FirstOrDefaultAsync();
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
            List<Transaction> transactions = mCollection.Find(x => x.walletId == walletId).ToList();
            return new Result
            {
                status = 200,
                data = transactions
            };
        }

        public async Task<Result> GetTransactionsByWalletAsync(string walletId)
        {
            List<Transaction> transactions = await mCollection.Find(x => x.walletId == walletId).ToListAsync();
            return new Result
            {
                status = 200,
                data = transactions
            };
        }
    }
}
