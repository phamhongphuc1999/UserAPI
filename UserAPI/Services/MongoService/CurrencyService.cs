// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class CurrencyService: BaseService<Currency>
    {
        private IconService iconService;
        public CurrencyService(string database, string collection): base(database)
        {
            mCollection = mDatabase.GetCollection<Currency>(collection);
            iconService = new IconService(database, "Icon");
        }

        public Result InsertCurrency(string iconId, string name)
        {
            Result result = iconService.GetIconById(iconId);
            if (result.data == null) return new Result
            {
                status = 400,
                data = $"the iconId: {iconId} do not exist"
            };
            Currency currency = mCollection.Find(x => x.name == name).ToList().FirstOrDefault();
            if (currency != null) return new Result
            {
                status = 400,
                data = $"the name: {name} already exist"
            };
            mCollection.InsertOne(new Currency
            {
                iconId = iconId,
                name = name
            });
            currency = mCollection.Find(x => x.name == name).ToList().FirstOrDefault();
            return new Result
            {
                status = 200,
                data = currency
            };
        }

        public async Task<Result> InsertCurrencyAsync(string iconId, string name)
        {
            Result result = await iconService.GetIconByIdAsync(iconId);
            if (result.data == null) return new Result
            {
                status = 400,
                data = $"the iconId: {iconId} do not exist"
            };
            Currency currency = await mCollection.Find(x => x.name == name).FirstOrDefaultAsync();
            if (currency == null) return new Result
            {
                status = 400,
                data = $"the name: {name} already exist"
            };
            mCollection.InsertOne(new Currency
            {
                iconId = iconId,
                name = name
            });
            currency = await mCollection.Find(x => x.name == name).FirstOrDefaultAsync();
            return new Result
            {
                status = 200,
                data = currency
            };
        }
    }
}
