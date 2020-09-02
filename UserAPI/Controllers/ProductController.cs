using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MongoDatabase.Entities;
using MongoDatabase.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IOptions<JWTConfig> _jwtConfig;
        private ProductModel productModel;

        public ProductController(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
            productModel = new ProductModel();
        }

        /// <summary>create new product</summary>
        /// <remarks>create new product</remarks>
        /// <returns></returns>
        /// <response code="200">return infomation of new product</response>
        /// <response code="400">if get mistake</response>
        [HttpPost("/products")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> CreateNewProduct()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string productInfo = await reader.ReadToEndAsync();
                Product newProduct = JsonConvert.DeserializeObject<Product>(productInfo);
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
        /// <response code="200">return infomation of product with specified fields</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> GetProductById(string productId)
        {
            try
            {
                string fieldsString = Request.Query["fields"];
                Result result;
                if (fieldsString != null)
                {
                    string[] fields = fieldsString.Split(',');
                    result = await productModel.GetProductById(productId, fields);
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
        /// <response code="200">return infomation of list products</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/products")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> GetListProduct()
        {
            try
            {
                string pageSize = Request.Query["page_size"];
                string pageIndex = Request.Query["page_index"];
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
        /// <response code="200">return infomation of product you updated</response>
        /// <response code="400">if get mistake</response>
        [HttpPut("/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> UpdateProduct(string productId)
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string productInfo = await reader.ReadToEndAsync();
                Product updateProduct = JsonConvert.DeserializeObject<Product>(productInfo);
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
        /// <response code="200">return infomation of product you deleted</response>
        /// <response code="400">if get mistake</response>
        [HttpDelete("/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
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
