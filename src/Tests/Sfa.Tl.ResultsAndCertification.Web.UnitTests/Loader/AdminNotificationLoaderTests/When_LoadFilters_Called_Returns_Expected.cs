using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public class When_LoadFilters_Called_Returns_Expected : AdminNotificationLoaderBaseTest
    {
        private AdminFindNotificationCriteriaViewModel _result;

        public override void Given()
        {
        }

        public override Task When()
        {
            _result = Loader.LoadFilters();
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            IList<FilterLookupData> filters = _result.ActiveFilters;

            filters.Should().NotBeNullOrEmpty();
            filters.Should().HaveCount(2);

            filters[0].Id.Should().Be(1);
            filters[0].Name.Should().Be("Yes");
            filters[0].IsSelected.Should().BeFalse();

            filters[1].Id.Should().Be(2);
            filters[1].Name.Should().Be("No");
            filters[1].IsSelected.Should().BeFalse();

            _result.AreFiltersApplied.Should().BeFalse();
            _result.PageNumber.Should().Be(1);
        }
    }
}