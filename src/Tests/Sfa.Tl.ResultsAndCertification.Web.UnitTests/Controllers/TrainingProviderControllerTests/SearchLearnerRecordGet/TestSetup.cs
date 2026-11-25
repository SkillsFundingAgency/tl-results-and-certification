using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordGet
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public IActionResult Result { get; private set; }

        protected readonly IList<AvailableStartYearViewModel> AvailableAcademicYears = new List<AvailableStartYearViewModel>
        {
            new() { Year = 2020, DisplayYear = "2020 to 2021" },
            new() { Year = 2021, DisplayYear = "2021 to 2022" }
        };

        public async override Task When()
        {
            Result = await Controller.SearchLearnerRecordAsync();
        }
    }
}
