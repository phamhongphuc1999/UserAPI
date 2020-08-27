namespace UserAPI
{
    public static class Responder
    {
        public static object Success(object data)
        {
            return new
            {
                status = "success",
                data = data
            };
        }

        public static object Fail(object reason)
        {
            return new
            {
                status = "fail",
                reason = reason
            };
        }
    }
}
