namespace Sample
{
    public static class CSharpNinePatternMatching
    {
        public static bool IsLetter(this char c) =>
            c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

        public static bool InRangeOld(this int i, int min, int max) =>
            i >= min && i < max;

        // public static bool InRange(this int i, int min, int max) =>
        //     i is >= min and < max;
    }
}