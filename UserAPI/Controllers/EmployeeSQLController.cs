using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDatabase.Entities;
using UserAPI.Models.SQLServer;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class EmployeeSQLController : ControllerBase
    {
        private EmployeeService employeeService;

        public EmployeeSQLController(IConfiguration configuration)
        {
            employeeService = new EmployeeService(configuration);
        }

        /// <summary>Create New Employee</summary>
        /// <remarks>Create New Employee</remarks>
        /// <param name="newEmployee"></param>
        /// <returns></returns>
        /// <response code="200">return infomation of new user</response>
        /// <response code="400">if get mistake</response>
        [HttpPost("/employees")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<ObjectResult> CreateNewEmployee([FromBody] InsertEmployeeInfo newEmployee)
        {
            try
            {
                Result result = await employeeService.InsertEmployee(newEmployee);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>Get Employee By Username</summary>
        /// <remarks>Get Employee By Username</remarks>
        /// <param name="username">the username of employee you want to get</param>
        /// <param name="fields">the specified fields you want to get</param>
        /// <response code="200">return infomation of user with specified fields</response>
        /// <response code="400">if get mistake</response>
        /// <returns></returns>
        [HttpGet("/employees/{username}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<ObjectResult> GetEmployeeByUsername(string username, [FromQuery] string fields)
        {
            try
            {
                Result result;
                if (fields != null)
                {
                    string[] fieldList = fields.Split(',');
                    result = await employeeService.GetEmployeeByUsername(username, fieldList);
                }
                else result = await employeeService.GetEmployeeByUsername(username);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>Get List Employee</summary>
        /// <remarks>Get List Employee</remarks>
        /// <param name="pageIndex">the page index you want to get</param>
        /// <param name="pageSize">the user per page you want to set</param>
        /// <returns></returns>
        /// <response code="200">return infomation of list employees</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/employees")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<ObjectResult> GetListEmployees([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            try
            {
                Result result = await employeeService.GetListEmployees(pageSize, pageIndex);
                return Ok(Responder.Success(result.data));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
