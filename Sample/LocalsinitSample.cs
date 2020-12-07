using System.Runtime.CompilerServices;

namespace Sample
{
    public class LocalsinitSample
    {
        public static int TestMethodNormal(int a)
        {
            int b = a + 3;
            return b;
        }

        [SkipLocalsInit]
        public static int TestMethodSkip(int a)
        {
            int c;
            int b = a + 3;
            return b;
        }

        [SkipLocalsInit]
        public static int TestMethodSkip2()
        {
            int i, j;
            return 1;
        }
    }
}