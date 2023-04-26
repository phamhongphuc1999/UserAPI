namespace UserAPI.Configuration
{
  public static class Messages
  {
    public static readonly string OK = "OK";
    public static readonly string BadRequest = "Bad Request";
    public static readonly string Unauthorized = "Unauthorized";
    public static readonly string Forbidden = "Forbidden";
    public static readonly string WrongUserPassword = "User or password wrong";
    public static readonly string EnableAccount = "This account is enable to login";
    public static readonly string ExistedUser = "This user have existed";
    public static readonly string NotExistedUser = "This user do not existed";
    public static readonly string InvalidToken = "Invalid Token";
  }

  public static class Status
  {
    public static readonly int OK = 200;
    public static readonly int Created = 201;

    public static readonly int BadRequest = 400;
    public static readonly int Unauthorized = 401;
    public static readonly int Forbidden = 403;
  }
}