using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations.DateValidatorAttributeTests
{
    public class When_Validation_Date_Greater_Than_Today : DateValidatorAttributeBaseTests
    {
        public override void Given()
        {
            model = new RequiredDateViewModel { Day = "01", Month = "01", Year = "2024" };
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
            overAllResult.Should().BeFalse();
            validationResults.Should().NotBeNull();
            validationResults.Should().HaveCount(1);
            validationResults[0].ErrorMessage.Should().Be(string.Format(ErrorResource.ReviewChangeStartYear.Validation_Date_When_Change_Requested_Future_Date_Text, model.RequiredDate));
        }
    }
}