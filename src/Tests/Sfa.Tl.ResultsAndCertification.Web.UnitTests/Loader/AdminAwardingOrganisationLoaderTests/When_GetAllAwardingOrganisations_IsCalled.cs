using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminAwardingOrganisationLoaderTests
{
    public class When_GetAllAwardingOrganisations_IsCalled : AdminAwardingOrganisationLoaderBaseTest
    {
        private readonly IEnumerable<AwardingOrganisationMetadata> _apiResult = new AwardingOrganisationMetadata[]
        {
            new()
            {
                Id = 3,
                Ukprn = 10009931,
                DisplayName = "City & Guilds"
            },
            new()
            {
                Id = 1,
                Ukprn = 10009696,
                DisplayName = "NCFE"
            },
            new()
            {
                Id = 2,
                Ukprn = 10022490,
                DisplayName = "Pearson"
            }
        };

        private AdminSelectAwardingOrganisationViewModel _result;

        public override void Given()
        {
            ApiClient.GetAllAwardingOrganisationsAsync().Returns(_apiResult);
        }

        public override async Task When()
        {
            _result = await Loader.GetSelectAwardingOrganisationViewModelAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.SelectedAwardingOrganisationUkprn.Should().NotHaveValue();
            _result.AwardingOrganisations.Should().BeEquivalentTo(_apiResult);

            _result.Breadcrumb.BreadcrumbItems.Should().HaveCount(2);
            _result.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(Breadcrumb.Home);
            _result.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.AdminHome);
            _result.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(Breadcrumb.Select_Awarding_Organisation);
        }
    }
}
