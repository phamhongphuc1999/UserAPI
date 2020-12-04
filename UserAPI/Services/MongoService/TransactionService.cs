// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class TransactionService: BaseService<Transaction>
    {
        public TransactionService(string database, string collection): base(database)
        {
            mCollection = mDatabase.GetCollection<Transaction>(collection);
        }

        public Result InsertTransaction(Transaction transaction)
        {
            mCollection.InsertOne(transaction);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertTransactionAsync(Transaction transaction)
        {
            await mCollection.InsertOneAsync(transaction);
            return new Result
            {
                status = 200,
                data = ""
            };
        }
    }
}
