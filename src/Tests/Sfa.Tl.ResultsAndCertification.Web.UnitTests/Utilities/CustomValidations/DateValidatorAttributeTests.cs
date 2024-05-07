using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations
{
    public class DateValidatorAttributeTests
    {
        [Theory]
        
        // Blank
        [InlineData("", "", "", false)]
        [InlineData("2024", "", "", false)]
        [InlineData("", "3", "", false)]
        [InlineData("", "", "1", false)]
        [InlineData("2024", "3", "", false)]
        [InlineData("2024", "", "1", false)]
        [InlineData("", "3", "1", false)]
        
        // Invalid
        [InlineData("2023", "13", "1", false)]
        [InlineData("2023", "2", "45", false)]
        [InlineData("2023", "-2", "1", false)]
        [InlineData("0", "2", "1", false)]
        [InlineData("2023", "2", "29", false)]
        [InlineData("2024", "2", "30", false)]
        [InlineData("999", "2", "30", false)]
        [InlineData("1752", "12", "31", false)]

        // Future
        [InlineData("2024", "3", "2", false)]
        [InlineData("2025", "1", "1", false)]

        // Valid
        [InlineData("2024", "3", "1", true)]
        [InlineData("2024", "03", "01", true)]
        [InlineData("2024", "02", "29", true)]
        [InlineData("1999", "12", "31", true)]
        [InlineData("1753", "1", "1", true)]
        public void Then_Returns_ExpectedResults(string year, string month, string day, bool isValid)
        {
            // Arrange
            var today = new DateTime(2024, 3, 1);

            var systemProvider = Substitute.For<ISystemProvider>();
            systemProvider.Today.Returns(today);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ISystemProvider)).Returns(systemProvider);

            var model = new DateValidatorAttributeTestModel(year, month, day);
            var validationContext = new ValidationContext(model, serviceProvider, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool result = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            result.Should().Be(isValid);

            if (isValid)
            {
                validationResults.Should().HaveCount(0);
            }
            else
            {
                validationResults.Should().HaveCount(1);
                validationResults[0].ErrorMessage.Should().NotBeNullOrEmpty();
            }
        }

        private class DateValidatorAttributeTestModel
        {
            private readonly string _year, _month, _day;

            public DateValidatorAttributeTestModel(string year, string month, string day)
                => (_year, _month, _day) = (year, month, day);

            [DateValidator(Property = nameof(DateToValidate), ErrorResourceType = typeof(ErrorResource.ReviewChangeStartYear))]
            public string DateToValidate => $"{_year}/{_month}/{_day}";
        }
    }
}