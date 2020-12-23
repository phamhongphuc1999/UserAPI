// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;

namespace UserAPI.Services.DataConnecters
{
    public class ElasticsearchConnecter
    {
        public ElasticClient Client { get; private set; }
        public ElasticsearchSetting CommonSetting { get; private set; }
        public ConnectionSettings ConnectConfig { get; private set; }
        private static ElasticsearchConnecter connecter;

        private ElasticsearchConnecter(IConfigurationSection configuration)
        {
            CommonSetting = new ElasticsearchSetting();
            configuration.Bind(CommonSetting);
            StaticConnectionPool connectionPool = new StaticConnectionPool(CommonSetting.Uris);
            ConnectConfig = new ConnectionSettings(connectionPool);
            Client = new ElasticClient(ConnectConfig);
        }

        public static ElasticsearchConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new ElasticsearchConnecter(configuration);
            return connecter;
        }
    }
}
