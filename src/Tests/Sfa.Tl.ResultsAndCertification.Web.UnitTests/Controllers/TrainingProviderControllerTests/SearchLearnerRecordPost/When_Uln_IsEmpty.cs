using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using Xunit;
using SearchUlnContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SearchLearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordPost
{
    public class When_Uln_IsEmpty : TestSetup
    {
        private readonly IList<AvailableStartYearViewModel> _availableAcademicYears = new List<AvailableStartYearViewModel>
        {
            new() { Year = 2020, DisplayYear = "2020 to 2021" }
        };

        public override void Given()
        {
            var uln = string.Empty;

            SearchLearnerRecordViewModel = new SearchLearnerRecordViewModel { SearchUln = uln };
            Controller.ModelState.AddModelError("SearchUln", SearchUlnContent.Uln_Required_Validation_Message);
            TrainingProviderLoader.GetAvailableAcademicYearsAsync().Returns(_availableAcademicYears);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchLearnerRecordViewModel));
            (viewResult.Model as SearchLearnerRecordViewModel).AvailableAcademicYears.Should().BeEquivalentTo(_availableAcademicYears);

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchLearnerRecordViewModel.SearchUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(SearchLearnerRecordViewModel.SearchUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(SearchUlnContent.Uln_Required_Validation_Message);
        }
    }
}
