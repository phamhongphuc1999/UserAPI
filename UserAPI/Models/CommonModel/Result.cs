namespace UserAPI.Models.CommonModel
{
  public class Result
  {
    public int status { get; set; }
    public object data { get; set; }
  }

  public class Result<TResult>
  {
    public int status { get; set; }
    public TResult data { get; set; }
  }
}
