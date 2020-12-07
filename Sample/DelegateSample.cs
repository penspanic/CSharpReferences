namespace Sample
{
    public class DelegateSample
    {
        public delegate int Bar(int a, int b);

        private static int BarMethod(int a, int b) => a + b;

        public static void InvokeDelegate()
        {
            Bar bar = BarMethod;
            int returnValue = bar.Invoke(1, 2);
        }

        public static unsafe void InvokeFunctionPointer()
        {
            delegate*<int, int, int> fp = &BarMethod;
            int returnValue = fp(1, 2);
        }
    }
}