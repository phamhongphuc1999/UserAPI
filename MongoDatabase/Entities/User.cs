﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MongoDatabase.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string username { get; set; }

        public string password { internal get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public string email { get; set; }

        public string birthday { get; set; }

        public string phone { get; set; }

        public string role { get; set; }

        public string createAt { get; set; }

        public string updateAt { get; set; }

        public string lastLogin { get; set; }

        public string status { get; set; }
    }

    public class NewUserInfo
    {
        [Required(ErrorMessage = "the username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        public string password { get; set; }

        [Required(ErrorMessage = "the name is required")]
        public string name { get; set; }

        [Required(ErrorMessage = "the location is required")]
        public string location { get; set; }

        [Required(ErrorMessage = "the email is required")]
        public string email { get; set; }

        [Required(ErrorMessage = "the phone is required")]
        public string phone { get; set; }

        public string birthday { get; set; }
    }

    public class UpdateUserInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public string phone { get; set; }
    }

    public class UpdateRoleUserInfo
    {
        public string role { get; set; }
        public string status { get; set; }
    }

    public class UserLoginInfo
    {
        [Required(ErrorMessage = "the username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        public string password { get; set; }
    }
}