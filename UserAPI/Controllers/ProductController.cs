using Microsoft.AspNetCore.Mvc;
using MongoDatabase.Entities;
using MongoDatabase.Models;
using System;
using System.Threading.Tasks;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private ProductModel productModel;

        public ProductController()
        {
            productModel = new ProductModel();
        }

        /// <summary>create new product</summary>
        /// <remarks>create new product</remarks>
        /// <returns></returns>
        /// <param name="newProduct">the information of new product you want to add in your database</param>
        /// <response code="200">return infomation of new product</response>
        /// <response code="400">if get mistake</response>
        [HttpPost("/products")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> CreateNewProduct([FromBody] NewProductInfo newProduct)
        {
            try
            {
                Result result = await productModel.InsertProduct(newProduct);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>get product by id</summary>
        /// <remarks>get product by id</remarks>
        /// <returns></returns>
        /// <param name="productId">the id of product you want to get</param>
        /// <param name="fields">the specified fields you want to get</param>
        /// <response code="200">return infomation of product with specified fields</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> GetProductById(string productId, [FromQuery] string fields)
        {
            try
            {
                Result result;
                if (fields != null)
                {
                    string[] fieldList = fields.Split(',');
                    result = await productModel.GetProductById(productId, fieldList);
                }
                else result = await productModel.GetProductById(productId);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>get list products</summary>
        /// <remarks>get list products</remarks>
        /// <returns></returns>
        /// <param name="pageIndex">the page index you want to get</param>
        /// <param name="pageSize">the user per page you want to set</param>
        /// <response code="200">return infomation of list products</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/products")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> GetListProduct([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            try
            {
                Result result = await productModel.GetListProduct();
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>update product</summary>
        /// <remarks>update product</remarks>
        /// <returns></returns>
        /// <param name="productId">the id of product you want to update</param>
        /// <param name="updateProduct">the info used to update</param>
        /// <response code="200">return infomation of product you updated</response>
        /// <response code="400">if get mistake</response>
        [HttpPut("/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> UpdateProduct(string productId, [FromBody] UpdateProductInfo updateProduct)
        {
            try
            {
                Result result = await productModel.UpdateProduct(productId, updateProduct);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>delete product</summary>
        /// <remarks>delete product</remarks>
        /// <returns></returns>
        /// <param name="productId">the id of product you want to update</param>
        /// <response code="200">return infomation of product you deleted</response>
        /// <response code="400">if get mistake</response>
        [HttpDelete("/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> DeleteProduct(string productId)
        {
            try
            {
                Result result = await productModel.DeleteProduct(productId);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
