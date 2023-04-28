using MySqlConnector;
using UserAPI.Models.MySqlModel.DataSet;

namespace UserAPI.Models.MySqlModel
{
  public class SqlData
  {
    public MySqlConnection connection { get; set; }
    public SqlDataSet<Employee> employee { get; set; }
    public SqlDataSet<Production> production { get; set; }

    public SqlData(MySqlConnection connection)
    {
      this.connection = connection;
      this.employee = new SqlDataSet<Employee>(connection);
      this.production = new SqlDataSet<Production>(connection);
    }
  }
}