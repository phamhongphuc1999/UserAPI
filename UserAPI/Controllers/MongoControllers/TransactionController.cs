using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.MongoService;

namespace UserAPI.Controllers.MongoControllers
{
    [ApiController]
    [Produces("application/json")]
    public class TransactionController : ControllerBase
    {
        private TransactionService transactionService;

        public TransactionController()
        {
            transactionService = new TransactionService("MoneyLover", "Transaction");
        }

        /// <summary>Create new transaction</summary>
        /// <remarks>create new transaction</remarks>
        /// <param name="newTransaction">The instance representing new transaction</param>
        /// <returns></returns>
        public async Task<object> CreateNewTransaction([FromBody] Transaction newTransaction)
        {
            bool check = (newTransaction.amount <= 0) && (newTransaction.date == null) && (newTransaction.expenseId == null);
            if (!check) return StatusCode(401, Responder.Fail("Fill complete"));
            Result result = await transactionService.InsertTransactionAsync(newTransaction);
            return StatusCode(result.status, Responder.Success(result.data));
        }
    }
}
