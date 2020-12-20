// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Ipfs.Http;
using Microsoft.Extensions.Configuration;

namespace UserAPI.Services.IpfsService
{
    public class IpfsConnecter
    {
        protected IpfsClient ipfs;
        protected IpfsSetting setting;
        
        public IpfsConnecter(IConfigurationSection configuration)
        {
            setting = new IpfsSetting();
            configuration.Bind(setting);
            ipfs = new IpfsClient();
        }

        public IpfsSetting Setting
        {
            get { return setting; }
        }

        public IpfsClient IPFS
        {
            get { return ipfs; }
        }
    }
}
