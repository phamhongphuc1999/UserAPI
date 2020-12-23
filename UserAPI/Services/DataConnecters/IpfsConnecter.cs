// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Ipfs.Http;
using Microsoft.Extensions.Configuration;

namespace UserAPI.Services.DataConnecters
{
    public class IpfsConnecter
    {
        public IpfsClient IPFS { get; private set; }
        public IpfsSetting Setting { get; private set; }
        private static IpfsConnecter connecter;

        private IpfsConnecter(IConfigurationSection configuration)
        {
            Setting = new IpfsSetting();
            configuration.Bind(Setting);
            IPFS = new IpfsClient();
        }

        public static IpfsConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new IpfsConnecter(configuration);
            return connecter;
        }
    }
}
