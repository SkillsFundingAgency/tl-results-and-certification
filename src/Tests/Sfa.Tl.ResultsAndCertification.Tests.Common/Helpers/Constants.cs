using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers
{
    public class Constants
    {
        // Entity Constants

        public const string CreatedByUser = "TestUser";
        public static DateTime CreatedOn = DateTime.UtcNow; // DateTime.Parse("2019/02/15 11:12:13");
        public const string ModifiedByUser = "ModifyingUser";
        public static DateTime ModifiedOn = DateTime.UtcNow; // DateTime.Parse("2019/02/16 01:02:03");
    }
}
