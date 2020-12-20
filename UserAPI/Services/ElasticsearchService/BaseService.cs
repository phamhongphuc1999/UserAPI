// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Nest;
using static UserAPI.Program;
using APIResult = UserAPI.Models.CommonModel.Result;

namespace UserAPI.Services.ElasticsearchService
{
    public class BaseService
    {
        public APIResult SaveDocument<TDocument>(TDocument data, Id id = null) where TDocument: class
        {
            IndexRequest<TDocument> document = new IndexRequest<TDocument>(data, IndexName.From<TDocument>(), id);
            IndexResponse response = elasConnecter.Client.Index(document);
            if (response.ServerError != null) return new APIResult
            {
                data = response.ServerError,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            if (response.OriginalException != null) return new APIResult
            {
                data = response.OriginalException,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            return new APIResult
            {
                data = document,
                status = 200
            };
        }

        public APIResult QueryDocument<TDocument>(Id id) where TDocument: class
        {
            GetResponse<TDocument> response = elasConnecter.Client.Get<TDocument>(new GetRequest<TDocument>(id));
            if (response.ServerError != null) return new APIResult
            {
                data = response.ServerError,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            if (response.OriginalException != null) return new APIResult
            {
                data = response.OriginalException,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            return new APIResult
            {
                data = response.Fields,
                status = 200
            };
        }
    }
}
