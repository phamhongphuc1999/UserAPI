using UserAPI.Models.MySqlModel.DataSet;
using System.Collections.Generic;

namespace UserAPI.Services.SqlService
{
  public class BaseService<T> where T : new()
  {
    private SqlDataSet<T> dataSet;

    public BaseService(SqlDataSet<T> dataSet)
    {
      this.dataSet = dataSet;
    }

    public List<T> SelectAll(string projects)
    {
      if (projects == null) return this.dataSet.SelectAll("*");
      return this.dataSet.SelectAll(projects);
    }

    public List<T> SelectWithFilter(string filterCommand, string projects)
    {
      List<T> rawData = projects == null ? this.dataSet.SelectWithFilter(filterCommand, "*") : this.dataSet.SelectWithFilter(filterCommand, projects);
      return rawData;
    }

    public List<T> InsertSingle(string insertedElement, string insertedValue)
    {
      return this.dataSet.InsertSingle(insertedElement, insertedValue);
    }
  }
}
