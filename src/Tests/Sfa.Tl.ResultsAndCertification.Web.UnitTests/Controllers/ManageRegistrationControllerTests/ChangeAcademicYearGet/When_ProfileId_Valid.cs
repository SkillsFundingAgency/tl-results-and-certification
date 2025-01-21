﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeAcademicYearGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private ChangeAcademicYearViewModel mockresult;
        public override void Given()
        {
            mockresult = new ChangeAcademicYearViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                AoUkprn = AoUkprn,
                Name = "John Smith",
                ProviderDisplayName = "Barnsley College (10000536)",
                PathwayDisplayName = "Digital Business Services",
                AcademicYear = 2020,
                AcademicYearToBe = 2021
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(AoUkprn, ProfileId).Returns(mockresult);
            RegistrationLoader.GetAcademicYearsAsync().Returns(new List<AcademicYear> {
                new AcademicYear { Year = 2020, Name = "2020/21" }
                , new AcademicYear { Year = 2021, Name = "2021/22" } });
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(AoUkprn, ProfileId);
            RegistrationLoader.Received(1).GetAcademicYearsAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeAcademicYearViewModel));

            var model = viewResult.Model as ChangeAcademicYearViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.AoUkprn.Should().Be(mockresult.AoUkprn);
            model.AcademicYear.Should().Be(mockresult.AcademicYear);
            model.AcademicYearToBe.Should().Be(mockresult.AcademicYearToBe);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
