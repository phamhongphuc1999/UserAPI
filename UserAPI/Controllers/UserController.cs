using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;
using MongoDB.Bson;

namespace UserAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private mongodb data;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            data = new mongodb();
        }

        [HttpPost]
        [Route("/users")]
        public async Task<object> CreateNewUser()
        {
            try
            {
                _logger.LogInformation("Create New User");
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User newUser = JsonConvert.DeserializeObject<User>(userInfo);
                bool result = await data.InsertUser(newUser);
                if (result) return Ok("success");
                else return BadRequest("fail");
            }
            catch (BsonException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("/users/{userId}")]
        public async Task<object> GetUSerById(string userId)
        {
            try
            {
                _logger.LogInformation("Get User By Id");
                //string filedsString = Request.Query["fileds"];
                //string[] fileds = filedsString.Split(',');

                User user = await data.GetUserById(userId);
                return user;
            }
            catch (BsonException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("/users")]
        public async Task<object> GetListUser()
        {
            try
            {
                string pageSize = Request.Query["page_size"];
                string pageIndex = Request.Query["page-index"];
                List<User> userList = await data.GetListUser();
                return userList;
            }
            catch (BsonException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete]
        [Route("/users/{userId}")]
        public async Task<object> DeleteUSer(string userId)
        {
            try
            {
                bool result = await data.DeleteUSer(userId);
                if (result) return "success";
                else return BadRequest(String.Format("Fail to delete user with Id: {0}", userId));
            }
            catch (BsonException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
