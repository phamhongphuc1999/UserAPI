using System;
using UserAPI.Models.MongoModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using MongoDB.Bson;
using UserAPI.Services.MongoService.MongoDataService;
using UserAPI.Configuration;

namespace UserAPI.Services.MongoService
{
  public class UserService
  {
    private UserDataService service;

    public UserService(string collection)
    {
      service = new UserDataService(collection);
    }

    public async Task<Result> Login(string username, string password)
    {
      FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter = builder.Eq("username", username);
      User user = await service.GetSingleUser(filter);
      if (user == null) return new Result
      {
        status = Status.Unauthorized,
        data = Messages.WrongUserPassword
      };
      string rawPassword = Utilities.CalculateSHA256Hash(password);
      if (user.password != rawPassword) return new Result
      {
        status = Status.Unauthorized,
        data = Messages.WrongUserPassword
      };
      if (!user.status) return new Result
      {
        status = Status.Forbidden,
        data = Messages.EnableAccount
      };
      bool check = await service.UpdateUser(filter, new UpdateUserInfo { lastLogin = DateTime.Now });
      if (check) return new Result
      {
        status = Status.OK,
        data = new HelperTokenUser
        {
          userId = user.id,
          username = user.username,
          email = user.email
        }
      };
      return new Result
      {
        status = Status.BadRequest,
        data = Messages.BadRequest
      };
    }

    public async Task<Result> Register(NewUserInfo entity)
    {
      bool result = await service.InsertUser(entity);
      if (!result) return new Result
      {
        status = Status.BadRequest,
        data = Messages.BadRequest
      };
      return new Result
      {
        status = Status.OK,
        data = Messages.OK
      };
    }
    public async Task<Result> GetUserById(string userId, string[] fields = null)
    {
      FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter = builder.Eq("_id", ObjectId.Parse(userId));
      User user = await service.GetSingleUser(filter, fields);
      if (user == null) return new Result
      {
        status = Status.BadRequest,
        data = Messages.NotExistedUser
      };
      return new Result
      {
        status = Status.OK,
        data = user
      };
    }

    public async Task<Result> GetUserByUserName(string username, string[] fields = null)
    {
      FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter = builder.Eq("username", username);
      User user = await service.GetSingleUser(filter, fields);
      if (user == null) return new Result
      {
        status = Status.BadRequest,
        data = Messages.NotExistedUser
      };
      return new Result
      {
        status = Status.OK,
        data = user
      };
    }
    public async Task<Result> GetListUser(int pageSize = 0, int pageIndex = 0, string[] fields = null)
    {
      List<User> userList = await service.GetListUsers(fields);
      int totalResult = userList.Count;
      if (pageSize == 0) pageSize = totalResult;
      if (pageIndex == 0) pageIndex = 1;
      int index = pageSize * (pageIndex - 1);
      List<User> result = new List<User>();
      if (index + pageSize <= totalResult) result = userList.GetRange(index, pageSize);
      else if (index < totalResult) result = userList.GetRange(index, totalResult - 1);
      return new Result
      {
        status = Status.OK,
        data = new
        {
          user_list = result,
          pagination = new
          {
            totalResult = totalResult,
            pageIndex = pageIndex,
            pageSize = pageSize
          }
        }
      };
    }

    public async Task<Result> UpdateUser(string userId, UpdateUserInfo updateUser)
    {
      FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter = builder.Eq("_id", userId);
      bool result = await service.UpdateUser(filter, updateUser);
      if (result) return new Result
      {
        status = Status.OK,
        data = result
      };
      else return new Result
      {
        status = Status.BadRequest,
        data = Messages.BadRequest
      };
    }

    public async Task<Result> DeleteUser(string userId)
    {
      bool result = await service.DeleteUser(userId);
      if (result) return new Result
      {
        status = Status.OK,
        data = result
      };
      else return new Result
      {
        status = Status.BadRequest,
        data = Messages.BadRequest
      };
    }
  }
}
