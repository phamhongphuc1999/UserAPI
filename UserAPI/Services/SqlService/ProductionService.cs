using UserAPI.Models.MySqlModel;
using UserAPI.Connector;
using System.Collections.Generic;
using UserAPI.Models.CommonModel;
using UserAPI.Configuration;

namespace UserAPI.Services.SqlService
{
  public class ProductionService : BaseService<Production>
  {
    public ProductionService() : base(APIConnection.SQL.sqlData.production) { }

    public Result GetProductionById(string productionId, string fields)
    {
      List<Production> result = this.SelectWithFilter($"WHERE id=${productionId}", fields);
      if (result.Count > 0) return new Result { status = Status.OK, data = result[0] };
      else return new Result { status = Status.BadRequest, data = null };
    }
  }
}
