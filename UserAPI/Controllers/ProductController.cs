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
