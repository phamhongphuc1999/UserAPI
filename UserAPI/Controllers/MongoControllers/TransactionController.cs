using Microsoft.AspNetCore.Mvc;
using System;
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
        /// <param name="walletId"></param>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        [HttpPost("/transactions/{walletId}")]
        public async Task<object> CreateNewTransaction(string walletId, [FromQuery] string expenseId, [FromBody] NewTransactionInfo newTransaction)
        {
            try
            {
                bool check = (newTransaction.amount <= 0) && (newTransaction.date == null);
                if (!check) return StatusCode(401, Responder.Fail("Fill complete"));
                Result result = await transactionService.InsertTransactionAsync(walletId, expenseId, newTransaction);
                return StatusCode(result.status, Responder.Success("success"));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet("/transactions/{transactionId}")]
        public async Task<object> GetTransactionById(string transactionId)
        {
            try
            {
                Result result = await transactionService.GetTransactionByIdAsync(transactionId);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success(result.data));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
