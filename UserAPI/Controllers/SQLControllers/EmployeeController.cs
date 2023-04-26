using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.CommonModel;
using UserAPI.Models.SQLServerModel;
using static UserAPI.Program;

namespace UserAPI.Controllers.SQLControllers
{
  [Produces("application/json")]
  [Consumes("application/json")]
  [ApiController]
  public class EmployeeController : ControllerBase
  {
    /// <summary>Create New Employee</summary>
    /// <remarks>Create New Employee</remarks>
    /// <param name="newEmployee"></param>
    /// <returns></returns>
    /// <response code="200">return information of new user</response>
    /// <response code="400">if get mistake</response>
    [HttpPost("/employees")]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<ObjectResult> CreateNewEmployee([FromBody] InsertEmployeeInfo newEmployee)
    {
      try
      {
        Result result = await employeeService.InsertEmployeeAsync(newEmployee);
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
    /// <response code="200">return information of user with specified fields</response>
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
          string[] fieldList = Utilities.SplitFields(fields);
          result = await employeeService.GetEmployeeByUsernameAsync(username, fieldList);
        }
        else result = await employeeService.GetEmployeeByUsernameAsync(username);
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
    /// <param name="fields">the specified fields you want to get</param>
    /// <returns></returns>
    /// <response code="200">return information of list employees</response>
    /// <response code="400">if get mistake</response>
    [HttpGet("/employees")]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<ObjectResult> GetListEmployees([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string fields)
    {
      try
      {
        Result result;
        if (fields != null)
        {
          string[] fieldList = Utilities.SplitFields(fields);
          result = await employeeService.GetListEmployeesAsync(pageSize, pageIndex, fieldList);
        }
        else result = await employeeService.GetListEmployeesAsync(pageSize, pageIndex);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Update Employee</summary>
    /// <remarks>Update Employee</remarks>
    /// <param name="employeeId"></param>
    /// <param name="updateEmployee"></param>
    /// <returns></returns>
    /// <response code="200">return information of updated employee</response>
    /// <response code="400">if get mistake</response>
    [HttpPut("/employees/{employeeId}")]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<ObjectResult> UpdateEmployee(int employeeId, [FromBody] InsertEmployeeInfo updateEmployee)
    {
      try
      {
        Result result = await employeeService.UpdateEmployeeAsync(employeeId, updateEmployee);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Delete Employee</summary>
    /// <remarks>Delete Employee</remarks>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    [HttpDelete("/employees/{employeeId}")]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<ObjectResult> DeleteEmployee(int employeeId)
    {
      try
      {
        Result result = await employeeService.DeleteEmployeeAsync(employeeId);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }
  }
}
