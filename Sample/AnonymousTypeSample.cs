using System;

namespace Sample
{
    public class AnonymousTypeSample
    {
        public static void MethodForIlDecompilation()
        {
            var twoMember1 = new {integer = 3, str = "$!@#!@%"};
            var twoMember2 = new {single = 3.5f, character = 'a'};
            Console.WriteLine(twoMember1);
            Console.WriteLine(twoMember2);
        }
    }
}