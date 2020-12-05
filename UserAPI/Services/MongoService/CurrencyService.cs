using MongoDB.Driver;
using System.Collections.Generic;
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
            iconService = new IconService("MoneyLover", "Icon");
        }

        public Result InsertCurrency(string iconId, string name)
        {
            Result result = iconService.GetIconById(iconId);
            if (result.data == null) return new Result
            {
                status = 400,
                data = $"the iconId: ${iconId} do not exist"
            };
            Currency check = mCollection.Find(x => x.name == name).ToList().FirstOrDefault();
            if (check != null) return new Result
            {
                status = 400,
                data = $"the name: ${name} already exist"
            };
            mCollection.InsertOne(new Currency
            {
                iconId = new MongoDBRef("Icon", iconId),
                name = name
            });
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> InsertCurrencyAsync(string iconId, string name)
        {
            Result result = await iconService.GetIconByIdAsync(iconId);
            if (result.data == null) return new Result
            {
                status = 400,
                data = $"the iconId: ${iconId} do not exist"
            };
            List<Currency> check = await mCollection.Find(x => x.name == name).ToListAsync();
            if (check.Count > 0) return new Result
            {
                status = 400,
                data = $"the name: ${name} already exist"
            };
            mCollection.InsertOne(new Currency
            {
                iconId = new MongoDBRef("Icon", iconId),
                name = name
            });
            return new Result
            {
                status = 200,
                data = ""
            };
        }
    }
}
