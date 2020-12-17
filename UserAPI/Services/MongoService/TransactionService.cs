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
using static UserAPI.Program;

namespace UserAPI.Services.MongoService
{
    public class TransactionService: BaseService<Transaction>
    {
        public TransactionService(string collection): base(collection)
        {
        }

        public Result InsertTransaction(string walletId, NewTransactionInfo newTransaction)
        {
            Result result = walletService.GetWalletById(walletId);
            if (result.status != 200) return result;
            Wallet wallet = (Wallet)result.data;
            result = categoryService.GetSingerCategoryById(newTransaction.categoryId);
            if (result.status != 200) return result;
            Category category = (Category)result.data;
            string type = category.typeCategory;
            Transaction transaction = new Transaction()
            {
                walletId = walletId,
                categoryId = newTransaction.categoryId,
                amount = newTransaction.amount,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
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
            result = await categoryService.GetSingerCategoryByIdAsync(newTransaction.categoryId);
            if (result.status != 200) return result;
            Category category = (Category)result.data;
            string type = category.typeCategory;
            Transaction transaction = new Transaction()
            {
                walletId = walletId,
                categoryId = newTransaction.categoryId,
                amount = newTransaction.amount,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
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

        public Result GetListTransactions(GetTransactionInfo transactionInfo, string username, string[] fields)
        {
            Result result = walletService.GetWalletsByUser(username);
            if (result.status != 200) return result;
            Wallet specifiedWallet = null;
            if (transactionInfo.walletId != null)
            {
                Result check = walletService.GetWalletById(transactionInfo.walletId);
                if (check.status != 200) return check;
                specifiedWallet = (Wallet)check.data;
            }
            List<Transaction> transactions = new List<Transaction>();
            List<Wallet> walletList = (List<Wallet>)result.data;
            if (specifiedWallet != null)
            {
                bool a = false;
                foreach (Wallet item in walletList)
                    if (item._id == specifiedWallet._id)
                    {
                        a = true;
                        transactions = mCollection.Find(x => x.walletId == specifiedWallet._id).ToList();
                        break;
                    }
                if (!a) return new Result
                {
                    status = 400,
                    data = $"the wallet with id: {specifiedWallet._id} not found"
                };
            }
            else
            {
                walletList.ForEach(x =>
                {
                    List<Transaction> list = mCollection.Find(transaction => transaction.walletId == x._id).ToList();
                    transactions.AddRange(list);
                });
            }
            if (transactionInfo.typeCategory != null)
            {
                transactions = transactions.Where(x =>
                {
                    Result tempT = categoryService.GetSingerCategoryById(x.categoryId);
                    Category category = (Category)(tempT.data);
                    return category.typeCategory == transactionInfo.typeCategory;
                }).ToList();
            }
            if (transactionInfo.dateFrom != null)
            {
                DateTime dateFrom = Helper.ConvertStringToTime(transactionInfo.dateFrom);
                transactions = transactions.Where(x => x.createAt.CompareTo(dateFrom) >= 0).ToList();
            }
            if (transactionInfo.dateTo != null)
            {
                DateTime dateTo = Helper.ConvertStringToTime(transactionInfo.dateTo);
                transactions = transactions.Where(x => x.createAt.CompareTo(dateTo) <= 0).ToList();
            }
            transactions = transactions.OrderByDescending(x => x.createAt).ToList();
            IEnumerable<Dictionary<string, object>> transactionValues = transactions.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string item in fields)
                    if (mongoConnecter.Setting.TransactionFields.Contains(item))
                        result.Add(item, e.GetType().GetProperty(item).GetValue(e));
                return result;
            });
            return new Result
            {
                status = 200,
                data = transactionValues
            };
        }

        public async Task<Result> GetListTransactionsAsync(GetTransactionInfo transactionInfo, string username, string[] fields)
        {
            Result result = await walletService.GetWalletsByUserAsync(username);
            if (result.status != 200) return result;
            Wallet specifiedWallet = null;
            if (transactionInfo.walletId != null)
            {
                Result check = await walletService.GetWalletByIdAsync(transactionInfo.walletId);
                if (check.status != 200) return check;
                specifiedWallet = (Wallet)check.data;
            }
            List<Transaction> transactions = new List<Transaction>();
            List<Wallet> walletList = (List<Wallet>)result.data;
            if (specifiedWallet != null)
            {
                bool a = false;
                foreach (Wallet item in walletList)
                    if (item._id == specifiedWallet._id)
                    {
                        a = true;
                        transactions = await mCollection.Find(x => x.walletId == specifiedWallet._id).ToListAsync();
                        break;
                    }
                if (!a) return new Result
                {
                    status = 400,
                    data = $"the wallet with id: {specifiedWallet._id} not found"
                };
            }
            else
            {
                walletList.ForEach(x =>
                {
                    List<Transaction> list = mCollection.Find(transaction => transaction.walletId == x._id).ToList();
                    transactions.AddRange(list);
                });
            }
            if (transactionInfo.typeCategory != null)
            {
                transactions = transactions.Where(x =>
                {
                    Task<Result> tempT = categoryService.GetSingerCategoryByIdAsync(x.categoryId);
                    tempT.Wait();
                    Category category = (Category)(tempT.Result.data);
                    return category.typeCategory == transactionInfo.typeCategory;
                }).ToList();
            }
            if (transactionInfo.dateFrom != null)
            {
                DateTime dateFrom = Helper.ConvertStringToTime(transactionInfo.dateFrom);
                transactions = transactions.Where(x => x.createAt.CompareTo(dateFrom) >= 0).ToList();
            }
            else if (transactionInfo.dateTo != null)
            {
                DateTime dateTo = Helper.ConvertStringToTime(transactionInfo.dateTo);
                transactions = transactions.Where(x => x.createAt.CompareTo(dateTo) <= 0).ToList();
            }
            transactions = transactions.OrderByDescending(x => x.createAt).ToList();
            IEnumerable<Dictionary<string, object>> transactionValues = transactions.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string item in fields)
                    if (mongoConnecter.Setting.TransactionFields.Contains(item))
                        result.Add(item, e.GetType().GetProperty(item).GetValue(e));
                return result;
            });
            return new Result
            {
                status = 200,
                data = transactionValues
            };
        }

        public Result UpdateTransaction(UpdateTransactionInfo updateTransaction, string transactionId, string username)
        {
            Transaction transaction = mCollection.Find(x => x._id == transactionId).FirstOrDefault();
            if (transaction == null) return new Result
            {
                status = 400,
                data = $"The transaction with id: {transactionId} not found"
            };
            Result result = walletService.GetWalletById(transaction.walletId);
            if (result.status != 200) return result;
            Wallet wallet = (Wallet)result.data;
            result = userService.GetUserById(wallet.userId);
            if (result.status != 200) return new Result
            {
                status = 403,
                data = "Forbidden"
            };
            UpdateDefinition<Transaction> builder = Builders<Transaction>.Update.Set(x => x.updateAt, DateTime.Now.Date);
            if (updateTransaction.amount > 0) builder = builder.Set(x => x.amount, updateTransaction.amount);
            if (updateTransaction.createAt != null) builder = builder.Set(x => x.createAt, Helper.ConvertStringToTime(updateTransaction.createAt));
            if (updateTransaction.note != null) builder = builder.Set(x => x.note, updateTransaction.note);
            mCollection.FindOneAndUpdate(x => x._id == transactionId, builder);
            transaction = mCollection.Find(x => x._id == transactionId).FirstOrDefault();
            return new Result
            {
                status = 200,
                data = transaction
            };
        }

        public async Task<Result> UpdateTransactionAsync(UpdateTransactionInfo updateTransaction, string transactionId, string username)
        {
            Transaction transaction = await mCollection.Find(x => x._id == transactionId).FirstOrDefaultAsync();
            if (transaction == null) return new Result
            {
                status = 400,
                data = $"The transaction with id: {transactionId} not found"
            };
            Result result = await walletService.GetWalletByIdAsync(transaction.walletId);
            if (result.status != 200) return result;
            Wallet wallet = (Wallet)result.data;
            result = await userService.GetUserByIdAsync(wallet.userId);
            if (result.status != 200) return new Result
            {
                status = 403,
                data = "Forbidden"
            };
            UpdateDefinition<Transaction> builder = Builders<Transaction>.Update.Set(x => x.updateAt, DateTime.Now.Date);
            if (updateTransaction.amount > 0) builder = builder.Set(x => x.amount, updateTransaction.amount);
            if (updateTransaction.createAt != null) builder = builder.Set(x => x.createAt, Helper.ConvertStringToTime(updateTransaction.createAt));
            if (updateTransaction.note != null) builder = builder.Set(x => x.note, updateTransaction.note);
            mCollection.FindOneAndUpdate(x => x._id == transactionId, builder);
            transaction = await mCollection.Find(x => x._id == transactionId).FirstOrDefaultAsync();
            return new Result
            {
                status = 200,
                data = transaction
            };
        }
    }
}
