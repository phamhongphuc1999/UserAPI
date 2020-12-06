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
    public class WalletService: BaseService<Wallet>
    {
        private UserService userService;

        public WalletService(string database, string collection): base(database)
        {
            mCollection = mDatabase.GetCollection<Wallet>(collection);
            userService = new UserService("MoneyLover", "User");
        }

        public Result InsertWallet(string userId, NewWalletInfo newWallet)
        {
            Result result = userService.GetUserById(userId);
            if (result.status != 200) return result;
            Wallet wallet = new Wallet()
            {
                userId = new MongoDBRef("User", ObjectId.Parse(userId)),
                currencyId = new MongoDBRef("Currency", ObjectId.Parse(newWallet.currencyId)),
                iconId = new MongoDBRef("Icon", ObjectId.Parse(newWallet.iconId)),
                name = newWallet.name,
                amount = newWallet.amount
            };
            mCollection.InsertOne(wallet);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertWalletAsync(string userId, NewWalletInfo newWallet)
        {
            Result result = await userService.GetUserByIdAsync(userId);
            if (result.status != 200) return result;
            Wallet wallet = new Wallet()
            {
                userId = new MongoDBRef("User", ObjectId.Parse(userId)),
                currencyId = new MongoDBRef("Currency", ObjectId.Parse(newWallet.currencyId)),
                iconId = new MongoDBRef("Icon", ObjectId.Parse(newWallet.iconId)),
                name = newWallet.name,
                amount = newWallet.amount
            };
            await mCollection.InsertOneAsync(wallet);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public Result GetWalletById(string walletId)
        {
            List<Wallet> wallets = mCollection.Find(x => x._id == walletId).ToList();
            Wallet wallet = wallets.FirstOrDefault();
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
            List<Wallet> wallets = await mCollection.Find(x => x._id == walletId).ToListAsync();
            Wallet wallet = wallets.FirstOrDefault();
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
            List<Wallet> wallets = mCollection.Find(x => x.userId.Id == BsonValue.Create(user._id)).ToList();
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
            List<Wallet> wallets = await mCollection.Find(x => x.userId.Id == BsonValue.Create(user._id)).ToListAsync();
            return new Result
            {
                status = 200,
                data = wallets
            };
        }
    }
}
