using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Entities;
using MongoDB.Models;
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
        private readonly ILogger<ProductController> _logger;
        private readonly IOptions<DevelopmentConfig> _developmentConfig;
        private readonly IOptions<JWTConfig> _jwtConfig;
        private ProductModel productModel;

        public ProductController(ILogger<ProductController> logger, IOptions<DevelopmentConfig> developmentConfig,
            IOptions<JWTConfig> jwtConfig)
        {
            _logger = logger;
            _developmentConfig = developmentConfig;
            _jwtConfig = jwtConfig;
            productModel = new ProductModel();
        }

        /// <summary>
        /// create new product
        /// </summary>
        /// <returns></returns>
        [HttpPost("/products")]
        public async Task<object> CreateNewProduct()
        {
            try
            {
                _logger.LogInformation("POST: {0}/users", _developmentConfig.Value.ApplicationUrl);
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string productInfo = await reader.ReadToEndAsync();
                Product newProduct = JsonConvert.DeserializeObject<Product>(productInfo);
                Result result = await productModel.InsertProduct(newProduct);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="productId">the product id which want to get</param>
        /// <returns></returns>
        [HttpGet("/products/{productId}")]
        public async Task<object> GetProductById(string productId)
        {
            try
            {
                _logger.LogInformation("GET: {0}/users/{1}", _developmentConfig.Value.ApplicationUrl, productId);
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
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// get list product
        /// </summary>
        /// <returns></returns>
        [HttpGet("/products")]
        public async Task<object> GetListProduct()
        {
            try
            {
                _logger.LogInformation("GET: {0}/users", _developmentConfig.Value.ApplicationUrl);
                string pageSize = Request.Query["page_size"];
                string pageIndex = Request.Query["page_index"];
                Result result = await productModel.GetListProduct();
                return Ok(Responder.Success(result.data));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// update product
        /// </summary>
        /// <param name="productId">the product id which want to update</param>
        /// <returns></returns>
        [HttpPut("/products/{productId}")]
        public async Task<object> UpdateProduct(string productId)
        {
            try
            {
                _logger.LogInformation("PUT: {0}/users/{1}", _developmentConfig.Value.ApplicationUrl, productId);
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string productInfo = await reader.ReadToEndAsync();
                Product updateProduct = JsonConvert.DeserializeObject<Product>(productInfo);
                Result result = await productModel.UpdateProduct(productId, updateProduct);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// delete a product
        /// </summary>
        /// <param name="productId">the product id which want to delete</param>
        /// <returns></returns>
        [HttpDelete("/products/{productId}")]
        public async Task<object> DeleteProduct(string productId)
        {
            try
            {
                _logger.LogInformation("DELETE: {0}/users/{1}", _developmentConfig.Value.ApplicationUrl, productId);
                Result result = await productModel.DeleteProduct(productId);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
