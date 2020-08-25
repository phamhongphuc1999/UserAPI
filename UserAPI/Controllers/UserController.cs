using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private mongodb data;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            data = new mongodb();
        }

        [HttpGet]
        public async Task<object> GetListUser()
        {
            try
            {
                List<User> userList = await data.GetListUser();
                return userList;
            }
            catch(Exception error)
            {
                return error.Message;
            }
        }

        [HttpPost]
        public async Task<string> InsertUser()
        {
            try
            {
                await data.InsertUser();
                return "success";
            }
            catch(Exception error)
            {
                return error.Message;
            }
        }
    }
}
