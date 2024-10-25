using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Common.Utils
{
    public static class Image
    {
        public static string GetResultSlipHeaderLogo() =>
            Path.Combine(Directory.GetCurrentDirectory(), "Resources", "images", "tLevel-logo.jpg");
    }
}
