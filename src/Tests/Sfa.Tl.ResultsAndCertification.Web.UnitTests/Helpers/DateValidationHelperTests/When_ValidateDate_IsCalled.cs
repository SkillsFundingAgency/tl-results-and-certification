using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.DateValidationHelperTests
{
    public class When_ValidateDate_IsCalled : TestSetup
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(string InputDate, Dictionary<string, string> expectedErrors)
        {
            var validationerrors = InputDate.ValidateDate(PropertyName);

            validationerrors.Count.Should().Be(expectedErrors.Count);

            foreach (var error in validationerrors)
            {
                error.Value.Should().Be(expectedErrors[error.Key]);
            }
        }
    }
}
