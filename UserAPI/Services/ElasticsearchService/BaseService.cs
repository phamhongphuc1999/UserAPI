using Elasticsearch.Net;
using Nest;
using System;

namespace UserAPI.Services.ElasticsearchService
{
    public class BaseService
    {
        private ElasticClient client;
        private Uri[] uris;

        public BaseService(Uri[] uris)
        {
            this.uris = uris;
            StaticConnectionPool connectionPool = new StaticConnectionPool(uris);
            ConnectionSettings settings = new ConnectionSettings(connectionPool);
            client = new ElasticClient(settings);
        }

        public ElasticClient Client
        {
            get { return client; }
        }

        public Uri[] Uris
        {
            get { return uris; }
        }

        public UserAPI.Models.CommonModel.Result SaveDocument<TDocument>(TDocument data, Id id = null) where TDocument: class
        {
            IndexRequest<TDocument> document = new IndexRequest<TDocument>(data, IndexName.From<TDocument>(), id);
            IndexResponse response = client.Index(document);
            if (response.ServerError != null) return new Models.CommonModel.Result
            {
                data = response.ServerError,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            if (response.OriginalException != null) return new Models.CommonModel.Result
            {
                data = response.OriginalException,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            return new Models.CommonModel.Result
            {
                data = document,
                status = 200
            };
        }

        public UserAPI.Models.CommonModel.Result QueryDocument<TDocument>(Id id) where TDocument: class
        {
            GetResponse<TDocument> response = client.Get<TDocument>(new GetRequest<TDocument>(id));
            if (response.ServerError != null) return new Models.CommonModel.Result
            {
                data = response.ServerError,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            if (response.OriginalException != null) return new Models.CommonModel.Result
            {
                data = response.OriginalException,
                status = response.ApiCall.HttpStatusCode ?? 400
            };
            return new Models.CommonModel.Result
            {
                data = response.Fields,
                status = 200
            };
        }
    }
}
