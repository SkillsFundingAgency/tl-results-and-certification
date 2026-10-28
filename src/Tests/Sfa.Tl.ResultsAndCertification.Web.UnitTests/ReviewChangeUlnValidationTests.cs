using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests
{
    public class ReviewChangeUlnValidationTests
    {
        [Theory]
        [InlineData(nameof(ReviewChangeUlnViewModel.ContactName), "", "Enter the name of the contact who requested this change")]
        [InlineData(nameof(ReviewChangeUlnViewModel.ChangeReason), "", "Enter the reason why this change is needed")]
        public void Field_validation_loads_dedicated_resource_messages(string property, string value, string expected)
        {
            var context = new ValidationContext(new ReviewChangeUlnViewModel()) { MemberName = property };
            var results = new List<ValidationResult>();

            Assert.False(Validator.TryValidateProperty(value, context, results));
            Assert.Equal(expected, Assert.Single(results).ErrorMessage);
        }

        [Theory]
        [InlineData("", "", "", "Enter the date when the change was requested")]
        [InlineData("31", "2", "2026", "Date must be a real date")]
        [InlineData("2", "1", "2026", "Date must be today or in the past")]
        public void Date_validation_loads_dedicated_resource_messages(string day, string month, string year, string expected)
        {
            var model = new ReviewChangeUlnViewModel { Day = day, Month = month, Year = year };
            var systemProvider = Substitute.For<ISystemProvider>();
            systemProvider.Today.Returns(new DateTime(2026, 1, 1));
            var services = Substitute.For<IServiceProvider>();
            services.GetService(typeof(ISystemProvider)).Returns(systemProvider);
            var context = new ValidationContext(model, services, null) { MemberName = nameof(model.RequestDate) };
            var results = new List<ValidationResult>();

            Assert.False(Validator.TryValidateProperty(model.RequestDate, context, results));
            Assert.Equal(expected, Assert.Single(results).ErrorMessage);
        }
    }
}
