using System;

namespace ETS.Library.JPush.util
{
    class Preconditions
    {
        public static void checkArgument(bool expression)
        {
            if (!expression)
            {
                throw new ArgumentNullException();
            }
        }
        public static void checkArgument(bool expression, object errorMessage)
        {
            if (!expression)
            {
                throw new ArgumentException(errorMessage.ToString());
            }
        }
    }
}
