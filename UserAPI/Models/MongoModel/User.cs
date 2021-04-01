// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string username { get; set; }

        public string password { internal get; set; }

        public string email { get; set; }

        public DateTime createAt { get; set; }

        public DateTime updateAt { get; set; }

        public DateTime lastLogin { get; set; }

        public bool status { get; set; }
    }

    public class NewUserInfo
    {
        [Required(ErrorMessage = "the username is required", AllowEmptyStrings = false)]
        [StringLength(200)]
        public string username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        [StringLength(200)]
        public string password { get; set; }

        [EmailAddress]
        public string email { get; set; }
    }

    public class UpdateUserInfo
    {
        [StringLength(200)]
        public string username { get; set; }

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        [StringLength(200)]
        public string password { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public DateTime updateAt { get; set; }

        public DateTime lastLogin { get; set; }
    }

    public class UserLoginInfo
    {
        [Required(ErrorMessage = "the username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        public string password { get; set; }
    }

    public class HeplerTokenUser
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }
}