// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class IconService: BaseService<Icon>
    {
        public IconService(string collection): base(collection)
        {
        }

        public Result InsertIcon(string url)
        {
            Icon icon = mCollection.Find(x => x.url == url).ToList().FirstOrDefault();
            if (icon != null) return new Result
            {
                status = 200,
                data = $"the {url} exist"
            };
            mCollection.InsertOne(new Icon { url = url });
            return new Result
            {
                status = 200,
                data = new
                {
                    url = url
                }
            };
        }

        public async Task<Result> InsertIconAsync(string url)
        {
            List<Icon> result = await mCollection.Find(x => x.url == url).ToListAsync();
            Icon icon = result.FirstOrDefault();
            if (icon != null) return new Result
            {
                status = 200,
                data = $"the {url} exist"
            };
            mCollection.InsertOne(new Icon { url = url });
            return new Result
            {
                status = 200,
                data = new
                {
                    url = url
                }
            };
        }

        public Result GetIconById(string id)
        {
            Icon icon = mCollection.Find(x => x._id == id).ToList().FirstOrDefault();
            if (icon == null) return new Result
            {
                status = 400,
                data = $"The icon with id: {id} do not exist"
            };
            return new Result
            {
                status = 200,
                data = icon
            };
        }

        public async Task<Result> GetIconByIdAsync(string id)
        {
            Icon icon = await mCollection.Find(x => x._id == id).FirstOrDefaultAsync();
            if (icon == null) return new Result
            {
                status = 400,
                data = $"The icon with id: {id} do not exist"
            };
            return new Result
            {
                status = 200,
                data = icon
            };
        }
    }
}
