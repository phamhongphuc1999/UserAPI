// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Nest;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.ElasticsearchModel
{
    public class SaveData<TDocument>
    {
        [Required]
        public TDocument data;

        public Id id;
    }
}
