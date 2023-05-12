using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.MySqlModel;
using System;
using UserAPI.Models.CommonModel;
using UserAPI.Services;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Microsoft.Extensions.Options;
using UserAPI.Configuration;

namespace UserAPI.Controllers.SqlControllers
{
  public class ProductionController : BaseSqlController
  {
    public ProductionController(IOptions<JWTConfig> jwtConfig) : base(jwtConfig) { }
  }
}
