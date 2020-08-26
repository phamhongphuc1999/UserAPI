using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;

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
        public async Task<object> GetUSerById(string userId)
        {
            try
            {
                User user = await data.GetUserById(userId);
                return user;
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }

        [HttpGet]
        public async Task<object> GetListUser()
        {
            try
            {
                List<User> userList = await data.GetListUser();
                int pageSize = -1, pageIndex = -1;
                bool checkPageSize = Int32.TryParse(Request.Query["page_size"], out pageSize);
                bool checkPageIndex = Int32.TryParse(Request.Query["page_index"], out pageIndex);
                if (!checkPageIndex || !checkPageSize) return BadRequest("page size and page index must be interger");
                return userList;
            }
            catch(Exception error)
            {
                return error.Message;
            }
        }
    }
}
