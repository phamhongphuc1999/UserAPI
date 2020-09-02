using System;

namespace MongoDB
{
    public static class Hepler
    {
        public static string CurrentTime()
        {
            return DateTime.Now.ToString("G");
        }
    }
}
