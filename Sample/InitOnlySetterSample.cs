namespace Sample
{
    public class InitOnlySetterSample
    {
        public string LegacyGetterOnlyProperty { get; }
        public string InitOnlyProperty { get; init; }

        public InitOnlySetterSample Create()
        {
            // var a = new InitOnlySetterSample() {LegacyGetterOnlyProperty = "###"};
            var b = new InitOnlySetterSample() {InitOnlyProperty = "###"};
            return b;
        }
    }
}