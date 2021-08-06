using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.SearchResultsGet
{
    public class When_PopulateUln_IsFalse : TestSetup
    {
        private readonly string _searchUln = "9875641231";

        public override void Given()
        {
            PopulateUln = false;
            CacheService.GetAndRemoveAsync<string>(Arg.Any<string>()).Returns(_searchUln);
        }

        [Fact]
        public void Then_SearchUln_IsNull()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchResultsViewModel));

            var model = viewResult.Model as SearchResultsViewModel;
            model.Should().NotBeNull();
            model.SearchUln.Should().BeNull();
        }
    }
}
