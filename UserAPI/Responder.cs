namespace UserAPI
{
    public static class Responder
    {
        public static object Success(object data = null)
        {
            if(data != null) return new
            {
                status = "success",
                data = data
            };
            return new
            {
                status = "success",
                data = "null"
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
