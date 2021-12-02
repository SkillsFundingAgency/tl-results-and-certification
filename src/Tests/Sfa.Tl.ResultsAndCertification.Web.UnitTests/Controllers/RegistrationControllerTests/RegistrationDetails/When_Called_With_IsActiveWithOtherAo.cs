using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RegistrationDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.RegistrationDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationDetails
{
    public class When_Called_With_IsActiveWithOtherAo : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;
        private Dictionary<string, string> _routeAttributes;
        private IList<AcademicYear> _academicYears;

        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                DateofBirth = DateTime.UtcNow,
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020,
                Status = RegistrationPathwayStatus.Active,
                IsActiveWithOtherAo = true
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, mockresult.ProfileId.ToString() } };
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 }, new AcademicYear { Id = 2, Name = "2021/22", Year = 2021 } };

            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId).Returns(mockresult);
            RegistrationLoader.GetAcademicYearsAsync().Returns(_academicYears);
        }

        [Fact]
        public void Then_SummaryStatus_ActionLink_Not_Shown()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RegistrationDetailsViewModel));

            var model = viewResult.Model as RegistrationDetailsViewModel;
            model.Should().NotBeNull();

            // Summary Status
            model.SummaryStatus.Should().NotBeNull();
            model.SummaryStatus.Title.Should().Be(RegistrationDetailsContent.Title_Status);
            model.SummaryStatus.Value.Should().Be(mockresult.Status.ToString());
            model.SummaryStatus.ActionText.Should().BeNull();
            model.SummaryStatus.RouteName.Should().BeNull();
            model.SummaryStatus.RouteAttributes.Should().BeNull();
        }
    }
}
