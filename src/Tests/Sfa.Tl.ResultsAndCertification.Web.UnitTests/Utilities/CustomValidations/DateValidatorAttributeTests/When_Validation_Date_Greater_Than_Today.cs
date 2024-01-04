using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations.DateValidatorAttributeTests
{
    public class When_Validation_Date_Greater_Than_Today : DateValidatorAttributeBaseTests
    {
        public override void Given()
        {
            model = new RequiredDateViewModel { Day = "01", Month = "01", Year = "2050" };
            validationContext = new ValidationContext(model);
        }

        public override Task When()
        {
            overAllResult = Validator.TryValidateObject(model, validationContext, validationResults, true);
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_ExpectedResults()
        {
            overAllResult.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }
    }
}