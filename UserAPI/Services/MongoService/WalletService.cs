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
    public class WalletService: BaseService<Wallet>
    {
        private UserService userService;

        public WalletService(IOptions<MongoSetting> options, string collection): base(options)
        {
            mCollection = mDatabase.GetCollection<Wallet>(collection);
            userService = new UserService(options, "User");
        }

        public Result InsertWallet(string username, NewWalletInfo newWallet)
        {
            Result result = userService.GetUserByUserName(username);
            if (result.status != 200) return result;
            User user = (User)result.data;
            Wallet check = mCollection.Find(x => x.name == newWallet.name).FirstOrDefault();
            if (check != null) return new Result
            {
                status = 400,
                data = $"The name: {newWallet.name} already exist"
            };
            Wallet wallet = new Wallet()
            {
                userId = user._id,
                currencyId = newWallet.currencyId,
                iconId = newWallet.iconId,
                name = newWallet.name,
                amount = newWallet.amount,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
            mCollection.InsertOne(wallet);
            wallet = mCollection.Find(x => x.name == newWallet.name).FirstOrDefault();
            return new Result
            {
                status = 200,
                data = wallet
            };
        }

        public async Task<Result> InsertWalletAsync(string username, NewWalletInfo newWallet)
        {
            Result result = await userService.GetUserByUserNameAsync(username);
            if (result.status != 200) return result;
            User user = (User)result.data;
            Wallet check = await mCollection.Find(x => x.name == newWallet.name).FirstOrDefaultAsync();
            if (check != null) return new Result
            {
                status = 400,
                data = $"The name: {newWallet.name} already exist"
            };
            Wallet wallet = new Wallet()
            {
                userId = user._id,
                currencyId = newWallet.currencyId,
                iconId = newWallet.iconId,
                name = newWallet.name,
                amount = newWallet.amount,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
            await mCollection.InsertOneAsync(wallet);
            wallet = mCollection.Find(x => x.name == newWallet.name).FirstOrDefault();
            return new Result
            {
                status = 200,
                data = wallet
            };
        }

        public Result GetWalletById(string walletId)
        {
            Wallet wallet = mCollection.Find(x => x._id == walletId).FirstOrDefault();
            if (wallet == null) return new Result
            {
                status = 401,
                data = $"The wallet with id: {walletId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = wallet
            };
        }

        public async Task<Result> GetWalletByIdAsync(string walletId)
        {
            Wallet wallet = await mCollection.Find(x => x._id == walletId).FirstOrDefaultAsync();
            if (wallet == null) return new Result
            {
                status = 401,
                data = $"The wallet with id: {walletId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = wallet
            };
        }

        public Result GetWalletsByUser(string username)
        {
            Result result = userService.GetUserByUserName(username);
            if (result.status != 200) return result;
            User user = (User)result.data;
            List<Wallet> wallets = mCollection.Find(x => x.userId == user._id).ToList();
            return new Result
            {
                status = 200,
                data = wallets
            };
        }

        public async Task<Result> GetWalletsByUserAsync(string username)
        {
            Result result = userService.GetUserByUserName(username);
            if (result.status != 200) return result;
            User user = (User)result.data;
            List<Wallet> wallets = await mCollection.Find(x => x.userId == user._id).ToListAsync();
            return new Result
            {
                status = 200,
                data = wallets
            };
        }

        public Result UpdateWallet(string walletId, UpdateWalletInfo updateWallet)
        {
            UpdateDefinition<Wallet> builder = Builders<Wallet>.Update.Set(x => x.updateAt, DateTime.Now);
            if (updateWallet.iconId != null) builder = builder.Set(x => x.iconId, updateWallet.iconId);
            else if (updateWallet.name != null) builder = builder.Set(x => x.name, updateWallet.name);
            else if (updateWallet.amount != default(double)) builder.Set(x => x.amount, updateWallet.amount);
            Wallet wallet = mCollection.FindOneAndUpdate(x => x._id == walletId, builder);
            if (wallet == null) return new Result
            {
                status = 400,
                data = $"The wallet with id: {walletId} not found"
            };
            return new Result
            {
                status = 200,
                data = wallet
            };
        }

        public async Task<Result> UpdateWalletAsync(string walletId, UpdateWalletInfo updateWallet)
        {
            UpdateDefinition<Wallet> builder = Builders<Wallet>.Update.Set(x => x.updateAt, DateTime.Now);
            if (updateWallet.iconId != null) builder = builder.Set(x => x.iconId, updateWallet.iconId);
            else if (updateWallet.name != null) builder = builder.Set(x => x.name, updateWallet.name);
            else if (updateWallet.amount != default(double)) builder.Set(x => x.amount, updateWallet.amount);
            Wallet wallet = await mCollection.FindOneAndUpdateAsync(x => x._id == walletId, builder);
            if (wallet == null) return new Result
            {
                status = 400,
                data = $"The wallet with id: {walletId} not found"
            };
            return new Result
            {
                status = 200,
                data = wallet
            };
        }
    }
}
