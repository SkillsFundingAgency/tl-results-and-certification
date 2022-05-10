namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class BoolExtensions
    {
        private static readonly string _yes = "Yes";
        private static readonly string _no = "No";

        public static string ToYesOrNoString(this bool value)
        {
            return value ? _yes : _no;
        }
    }
}