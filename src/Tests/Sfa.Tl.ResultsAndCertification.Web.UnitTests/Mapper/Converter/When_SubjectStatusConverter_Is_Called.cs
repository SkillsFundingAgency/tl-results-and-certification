using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Converter;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Mapper.Converter
{
    public class When_SubjectStatusConverter_Is_Called
    {
        private readonly SubjectStatusConverter _converter = new();

        [Theory]
        [InlineData(null, "Not achieved")]
        [InlineData(SubjectStatus.NotSpecified, "Not achieved")]
        [InlineData(SubjectStatus.Achieved, "Achieved")]
        [InlineData(SubjectStatus.NotAchieved, "Not achieved")]
        [InlineData(SubjectStatus.AchievedByLrs, "Achieved")]
        [InlineData(SubjectStatus.NotAchievedByLrs, "Not achieved")]
        public void When_NotSpecified_Return_Expected(SubjectStatus? status, string expected)
            => _converter
                .Convert(status, null)
                .Should().Be(expected);
    }
}