using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations.RequiredWithMessageAttributeTests
{
    public class When_Validation_IsTriggered : BaseTest<RequiredWithMessageAttribute>
    {
        private RequiredWithMessageModel model;
        private ValidationContext validationContext;
        private List<ValidationResult> validationResults;
        private bool overAllResult;

        public override void Setup()
        {
            validationResults = new List<ValidationResult>();
        }

        public override void Given()
        {
            model = new RequiredWithMessageModel { Name = "John Smith", ValidData = "Test input" };
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
            validationResults.Should().HaveCount(2);
            validationResults[0].ErrorMessage.Should().Be(string.Format(ErrorResource.AddCoreAssessmentEntry.Select_Option_To_Add_Validation_Text, model.Name));
            validationResults[1].ErrorMessage.Should().Be(string.Format(ErrorResource.AddCoreAssessmentEntry.Select_Option_To_Add_Validation_Text, model.Name));
        }

        class RequiredWithMessageModel
        {
            public string Name { get; set; }

            [RequiredWithMessage(Property = nameof(Name), ErrorResourceType = typeof(ErrorResource.AddCoreAssessmentEntry), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
            public bool? HasValue { get; set; }

            [RequiredWithMessage(Property = nameof(Name), ErrorResourceType = typeof(ErrorResource.AddCoreAssessmentEntry), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
            public string Data { get; set; }

            [RequiredWithMessage(Property = nameof(Name), ErrorResourceType = typeof(ErrorResource.AddCoreAssessmentEntry), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
            public string ValidData { get; set; }
        }
    }
}