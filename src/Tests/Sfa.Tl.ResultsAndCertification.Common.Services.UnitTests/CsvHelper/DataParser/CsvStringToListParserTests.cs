using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.DataParser
{
    public class CsvStringToListParserTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(string input, IList<string> expected)
        {
            // Act
            IList<string> result = CsvStringToListParser.Parse(input);

            // Asset
            result.Should().BeEquivalentTo(expected);
        }

        public static IEnumerable<object[]> Data
            => new[]
            {
                // Null, empty or white space
                new object[] { null, new List<string>() },
                new object[] { string.Empty, new List<string>() },
                new object[] { " ", new List<string>() },
                new object[] { "  ", new List<string>() },

                // Only commas
                new object[] { ",", new List<string> { string.Empty, string.Empty } },
                new object[] { " ,  ", new List<string> { string.Empty, string.Empty } },
                new object[] { " ,  ,", new List<string> { string.Empty, string.Empty, string.Empty } },
                new object[] { @""""",  ""  """, new List<string> { string.Empty, string.Empty } },

                // Single item
                new object[] { "12345", new List<string>{"12345"} },
                new object[] { " 12345  ", new List<string>{"12345"} },
                new object[] { @"""12345""", new List<string>{"12345"} },
                new object[] { @"""""12345""""", new List<string>{"12345"} },
                new object[] { @""""" 12345  """"", new List<string>{"12345"} },
                new object[] { @" ""12345""  ", new List<string>{"12345"} },

                // Two items
                new object[] { "12345,67890", new List<string>{"12345", "67890" } },
                new object[] { " 12345 ,  67890    ", new List<string>{"12345", "67890" } },
                new object[] { @"""12345,67890""", new List<string>{"12345", "67890" } },
                new object[] { @"""12345 ,  67890""", new List<string>{"12345", "67890" } },
                new object[] { @" ""12345,67890""  ", new List<string>{"12345", "67890" } }
            };
    }
}