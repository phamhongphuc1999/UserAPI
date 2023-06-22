using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAPI.Configuration;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services;

namespace UserAPI.Controllers.MongoControllers
{
  public class ProductController : BaseMongoController
  {
    public ProductController(IOptions<JWTConfig> jwtConfig) : base(jwtConfig)
    {
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <remarks>Create a new product</remarks>
    /// <param name="entity">The data for creating a new product</param>
    /// <returns></returns>
    /// <response code="200">Return information of the new product</response>
    /// <response code="400">Return error when it occur</response>
    [HttpPost("/product")]
    [CustomAuthorization]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<object> InsertOneProduct([FromBody] InsertProduct entity)
    {
      try
      {
        string token = HttpContext.Request.Headers["token"];
        List<Claim> claims = authService.GetTokenClaims(token).ToList();
        string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
        Result data = await ServiceSelector.Mongo.user.GetUserByUserName(username, new string[] { "username" });
        User user = (User)data.data;
        Result result = await ServiceSelector.Mongo.product.AddNewProduct(entity, user.id);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <remarks>Get product by id</remarks>
    /// <param name="productId">The product id</param>
    /// <param name="fields">The specified fields you want to get</param>
    /// <returns></returns>
    /// <response code="200">Return information of product respective with specified fields</response>
    /// <response code="400">Return error when it occur</response>
    [HttpGet("/product/{productId}")]
    [CustomAuthorization]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<object> GetProductById(string productId, [FromQuery] string fields)
    {
      try
      {
        Result result;
        if (fields == null) result = await ServiceSelector.Mongo.product.GetProductById(productId);
        else
        {
          string[] fieldList = Utilities.SplitFields(fields);
          result = await ServiceSelector.Mongo.product.GetProductById(productId, fieldList);
        }
        if (result.status == 200) return Ok(Responder.Success(result.data));
        return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error));
      }
    }
  }
}
