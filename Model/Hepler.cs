using System;
using System.Collections.Generic;
using System.Text;

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
