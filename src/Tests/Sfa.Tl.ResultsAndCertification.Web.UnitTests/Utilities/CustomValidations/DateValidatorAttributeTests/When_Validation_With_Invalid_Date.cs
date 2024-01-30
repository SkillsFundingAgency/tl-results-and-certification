using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations.DateValidatorAttributeTests
{
    public class When_Validation_With_Invalid_Date : DateValidatorAttributeBaseTests
    {
        public override void Given()
        {
            model = new RequiredDateViewModel { Day = "32", Month = "12", Year = "2023" };
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
            validationResults[0].ErrorMessage.Should().Be(string.Format(ErrorResource.ReviewChangeStartYear.Validation_Date_When_Change_Requested_Invalid_Text, model.RequiredDate));          
        }
    }
}