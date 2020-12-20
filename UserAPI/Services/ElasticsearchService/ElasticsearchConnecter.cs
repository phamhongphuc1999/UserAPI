// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;

namespace UserAPI.Services.ElasticsearchService
{
    public class ElasticsearchConnecter
    {
        protected ElasticClient client;
        protected ElasticsearchSetting commonSetting;
        protected ConnectionSettings connectConfig;

        public ElasticsearchConnecter(IConfigurationSection configuration)
        {
            commonSetting = new ElasticsearchSetting();
            configuration.Bind(commonSetting);
            StaticConnectionPool connectionPool = new StaticConnectionPool(commonSetting.Uris);
            connectConfig = new ConnectionSettings(connectionPool);
            client = new ElasticClient(connectConfig);
        }

        public ElasticClient Client
        {
            get { return client; }
        }

        public ElasticsearchSetting CommonSetting
        {
            get { return commonSetting; }
        }

        public ConnectionSettings ConnectConfig
        {
            get { return connectConfig; }
        }
    }
}
