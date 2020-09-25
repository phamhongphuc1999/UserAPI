using MongoDatabase.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDatabase.Models
{
    public class UserModel : BaseModel<User>
    {
        public UserModel() : base()
        {
            mCollection = mDatabase.GetCollection<User>("user_list");
        }

        public Result Login(string username, string password)
        {
            User user = mCollection.Find(x => x.username == username).FirstOrDefault();
            if (user == null) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            string rawPassword = SHA256Hash.CalcuteHash(password);
            if (user.password != rawPassword) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            if (user.status == "disable") return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.lastLogin, Hepler.CurrentTime());
            mCollection.FindOneAndUpdate(x => x.username == username, updateBuilder);
            return new Result
            {
                status = 200,
                data = user.role
            };
        }

        public async Task<Result> LoginAsync(string username, string password)
        {
            User user = await mCollection.Find(x => x.username == username).FirstOrDefaultAsync();
            if (user == null) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            string rawPassword = SHA256Hash.CalcuteHash(password);
            if (user.password != rawPassword) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            if (user.status == "disable") return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.lastLogin, Hepler.CurrentTime());
            await mCollection.FindOneAndUpdateAsync(x => x.username == username, updateBuilder);
            return new Result
            {
                status = 200,
                data = user.role
            };
        }

        public Result InsertUser(NewUserInfo entity)
        {
            User user = mCollection.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            User newUser = new User()
            {
                username = entity.username,
                password = SHA256Hash.CalcuteHash(entity.password),
                name = entity.name,
                location = entity.location,
                email = entity.email,
                phone = entity.phone,
                role = "user",
                birthday = entity.birthday,
                createAt = Hepler.CurrentTime(),
                updateAt = Hepler.CurrentTime(),
                status = "enable"
            };
            mCollection.InsertOne(newUser);
            return new Result
            {
                status = 200,
                data = newUser
            };
        }

        public async Task<Result> InsertUserAsync(NewUserInfo entity)
        {
            User user = mCollection.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            User newUser = new User()
            {
                username = entity.username,
                password = SHA256Hash.CalcuteHash(entity.password),
                name = entity.name,
                location = entity.location,
                email = entity.email,
                phone = entity.phone,
                role = "user",
                birthday = entity.birthday,
                createAt = Hepler.CurrentTime(),
                updateAt = Hepler.CurrentTime(),
                status = "enable"
            };
            await mCollection.InsertOneAsync(newUser);
            return new Result
            {
                status = 200,
                data = newUser
            };
        }

        public Result GetUserById(string userId, string[] fields = null)
        {
            List<User> result = mCollection.Find(x => x._id == userId).ToList();
            User user = result.FirstOrDefault();
            if (user == null) return new Result
            {
                status = 400,
                data = $"the user with id: {userId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = user
            };
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string field in fields)
                if (Config.userFields.ContainsKey(field))
                    data.Add(field, result.GetType().GetProperty(field).GetValue(result));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetUserByIdAsync(string userId, string[] fields = null)
        {
            List<User> result = await mCollection.Find(x => x._id == userId).ToListAsync();
            User user = result.FirstOrDefault();
            if (user == null) return new Result
            {
                status = 400,
                data = $"the user with id: {userId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = user
            };
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string field in fields)
                if (Config.userFields.ContainsKey(field))
                    data.Add(field, result.GetType().GetProperty(field).GetValue(result));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public Result GetListUser(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<User> userList = mCollection.Find(x => x.name != String.Empty).ToList();
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = 200,
                data = new
                {
                    user_list = userList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
            List<User> tempList = userList.GetRange(index, pageSize);
            IEnumerable<Dictionary<string, object>> productFilterList = tempList.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add(field, value);
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = productFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public async Task<Result> GetListUserAsync(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<User> userList = await mCollection.Find(x => x.name != String.Empty).ToListAsync();
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = 200,
                data = new
                {
                    user_list = userList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                } 
            };
            List<User> tempList = userList.GetRange(index, pageSize);
            IEnumerable<Dictionary<string, object>> productFilterList = tempList.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add(field, value);
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = productFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public Result UpdateUser(string username, UpdateUserInfo updateUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateUser.username != null)
            {
                User checkUser = mCollection.Find(x => x.username == updateUser.username).ToList().FirstOrDefault();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.username, updateUser.username);
            }
            if (updateUser.name != null) updateBuilder = updateBuilder.Set(x => x.name, updateUser.name);
            if (updateUser.location != null) updateBuilder = updateBuilder.Set(x => x.location, updateUser.location);
            if (updateUser.birthday != null) updateBuilder = updateBuilder.Set(x => x.birthday, updateUser.birthday);
            if (updateUser.phone != null) updateBuilder = updateBuilder.Set(x => x.phone, updateUser.phone);
            if (updateUser.password != null)
            {
                string newPassword = SHA256Hash.CalcuteHash(updateUser.password);
                updateBuilder = updateBuilder.Set(x => x.password, newPassword);
            }
            User user = mCollection.FindOneAndUpdate(x => x.username == username, updateBuilder);
            if (user != null)
            {
                user = mCollection.Find(x => x._id == user._id).FirstOrDefault();
                return new Result
                {
                    status = 200,
                    data = user
                };
            }
            else return new Result
            {
                status = 400,
                data = $"do not update user with username: {username}"
            };
        }

        public async Task<Result> UpdateUserAsync(string username, UpdateUserInfo updateUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateUser.username != null)
            {
                User checkUser = mCollection.Find(x => x.username == updateUser.username).ToList().FirstOrDefault();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.username, updateUser.username);
            }
            if (updateUser.name != null) updateBuilder = updateBuilder.Set(x => x.name, updateUser.name);
            if (updateUser.location != null) updateBuilder = updateBuilder.Set(x => x.location, updateUser.location);
            if (updateUser.birthday != null) updateBuilder = updateBuilder.Set(x => x.birthday, updateUser.birthday);
            if (updateUser.phone != null) updateBuilder = updateBuilder.Set(x => x.phone, updateUser.phone);
            if (updateUser.password != null)
            {
                string newPassword = SHA256Hash.CalcuteHash(updateUser.password);
                updateBuilder = updateBuilder.Set(x => x.password, newPassword);
            }
            User user = await mCollection.FindOneAndUpdateAsync(x => x.username == username, updateBuilder);
            if (user != null)
            {
                user = mCollection.Find(x => x._id == user._id).FirstOrDefault();
                return new Result
                {
                    status = 200,
                    data = user
                };
            }
            else return new Result
            {
                status = 400,
                data = $"do not update user with username: {username}"
            };
        }

        public Result UpdateRole(string userId, UpdateRoleUserInfo updateRoleUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateRoleUser.role != null) updateBuilder.Set(x => x.role, updateRoleUser.role);
            if (updateRoleUser.status != null)
            {
                if (Config.userStatus.ContainsKey(updateRoleUser.status))
                    updateBuilder.Set(x => x.status, updateRoleUser.status);
                else return new Result
                {
                    status = 422,
                    data = $"Invalied value status: {updateRoleUser.status}"
                };
            }
            User user = mCollection.FindOneAndUpdate(x => x._id == userId, updateBuilder);
            if (user != null)
            {
                user = mCollection.Find(x => x._id == user._id).FirstOrDefault();
                return new Result
                {
                    status = 200,
                    data = user
                };
            }
            else return new Result
            {
                status = 400,
                data = $"do not update role user with id: {userId}"
            };
        }

        public async Task<Result> UpdateRoleAsync(string userId, UpdateRoleUserInfo updateRoleUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateRoleUser.role != null) updateBuilder.Set(x => x.role, updateRoleUser.role);
            if (updateRoleUser.status != null)
            {
                if (Config.userStatus.ContainsKey(updateRoleUser.status))
                    updateBuilder.Set(x => x.status, updateRoleUser.status);
                else return new Result
                {
                    status = 422,
                    data = $"Invalied value status: {updateRoleUser.status}"
                };
            }
            User user = await mCollection.FindOneAndUpdateAsync(x => x._id == userId, updateBuilder);
            if (user != null)
            {
                user = mCollection.Find(x => x._id == user._id).FirstOrDefault();
                return new Result
                {
                    status = 200,
                    data = user
                };
            }
            else return new Result
            {
                status = 400,
                data = $"do not update role user with id: {userId}"
            };
        }

        public Result DeleteUser(string userId)
        {
            User user = mCollection.FindOneAndDelete(x => x._id == userId);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {userId}"
            };
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            User user = await mCollection.FindOneAndDeleteAsync(x => x._id == userId);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {userId}"
            };
        }
    }
}
