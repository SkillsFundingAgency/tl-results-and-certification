namespace Sfa.Tl.ResultsAndCertification.Common.Utils
{
    public readonly struct InputDate
    {
        public string Day { get; }

        public string Month { get; }

        public string Year { get; }

        public InputDate(string day, string month, string year)
        {
            Day = day;
            Month = month;
            Year = year;
        }
    }
}