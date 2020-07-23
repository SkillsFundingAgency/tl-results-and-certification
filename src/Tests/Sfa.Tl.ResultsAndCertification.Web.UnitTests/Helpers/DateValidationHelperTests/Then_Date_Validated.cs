using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.DateValidationHelperTests
{
    public class Then_Date_Validated : When_ValidateDate_Is_Called
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void When_DateValidation_Is_Called_Then_Expected_Results_Returned(string InputDate, Dictionary<string, string> expectedErrors)
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
