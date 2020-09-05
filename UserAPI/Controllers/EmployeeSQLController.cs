using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.SQLServer;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class EmployeeSQLController : ControllerBase
    {
        private EmployeeDbContext employeeModel;

        public EmployeeSQLController(EmployeeDbContext context)
        {
            employeeModel = context;
        }

        [HttpGet("/employees")]
        public async Task<ObjectResult> GetListEmployees()
        {
            return Ok(Responder.Success(await employeeModel.GetListEmployees()));
        }
    }
}
