using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUniqueLearnerNumberGet
{
    public class When_Cache_IsFound : TestSetup
    {
        private Address _address = null;
        private RequestSoaUniqueLearnerNumberViewModel _cacheResult;
        public override void Given()
        {
            _address = new Address
            {
                DepartmentName = "Dept",
                OrganisationName = "Org",
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                Town = "Town",
                Postcode = "x11 1yy"
            };

            ResultsAndCertificationConfiguration.SoaAvailableDate = DateTime.UtcNow.AddDays(-30);
            ProviderAddressLoader.GetAddressAsync<Address>(ProviderUkprn).Returns(_address);

            _cacheResult = new RequestSoaUniqueLearnerNumberViewModel { SearchUln = "1234567890" };
            CacheService.GetAsync<RequestSoaUniqueLearnerNumberViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as RequestSoaUniqueLearnerNumberViewModel;
            model.SearchUln.Should().Be(_cacheResult.SearchUln);

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RequestStatementOfAchievement);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Request_Statement_Of_Achievement);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
        }
    }
}
