using System;

namespace Model
{
    public static class Hepler
    {
        public static string CurrentTime()
        {
            return DateTime.Now.ToString("G");
        }
    }
}
