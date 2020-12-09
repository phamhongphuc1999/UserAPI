// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.Extensions.Options;
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
        private CategoryService categoryService;

        public TransactionService(IOptions<MongoSetting> options, string collection): base(options)
        {
            mCollection = mDatabase.GetCollection<Transaction>(collection);
            walletService = new WalletService(options, "Wallet");
            categoryService = new CategoryService(options, "Category");
        }

        public Result InsertTransaction(string walletId, NewTransactionInfo newTransaction)
        {
            Result result = walletService.GetWalletById(walletId);
            if (result.status != 200) return result;
            Wallet wallet = (Wallet)result.data;
            result = categoryService.GetCategoryById(newTransaction.categoryId);
            if (result.status != 200) return result;
            Category category = (Category)result.data;
            string type = category.typeCategory;
            Transaction transaction = new Transaction()
            {
                walletId = walletId,
                categoryId = newTransaction.categoryId,
                amount = newTransaction.amount,
                createAt = DateTime.Now,
                note = newTransaction.note
            };
            if (type != "Income" && type != "Deat")
            {
                if (newTransaction.amount > wallet.amount) return new Result
                {
                    status = 400,
                    data = $"Not enough money"
                };
                mCollection.InsertOne(transaction);
                walletService.UpdateWallet(walletId, new UpdateWalletInfo { amount = wallet.amount - newTransaction.amount });
            }
            else
            {
                mCollection.InsertOne(transaction);
                walletService.UpdateWallet(walletId, new UpdateWalletInfo { amount = wallet.amount + newTransaction.amount });
            }
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
            Wallet wallet = (Wallet)result.data;
            result = await categoryService.GetCategoryByIdAsync(newTransaction.categoryId);
            if (result.status != 200) return result;
            Category category = (Category)result.data;
            string type = category.typeCategory;
            Transaction transaction = new Transaction()
            {
                walletId = walletId,
                categoryId = newTransaction.categoryId,
                amount = newTransaction.amount,
                createAt = DateTime.Now,
                note = newTransaction.note
            };
            if (type != "Income" && type != "Deat")
            {
                if (newTransaction.amount > wallet.amount) return new Result
                {
                    status = 400,
                    data = $"Not enough money"
                };
                mCollection.InsertOne(transaction);
                walletService.UpdateWallet(walletId, new UpdateWalletInfo { amount = wallet.amount - newTransaction.amount });
            }
            else
            {
                mCollection.InsertOne(transaction);
                walletService.UpdateWallet(walletId, new UpdateWalletInfo { amount = wallet.amount + newTransaction.amount });
            }
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
