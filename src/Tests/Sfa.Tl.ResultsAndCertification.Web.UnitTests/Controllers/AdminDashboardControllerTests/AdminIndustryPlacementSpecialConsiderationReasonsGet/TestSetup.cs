using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminIndustryPlacementSpecialConsiderationReasonsAsync();
        }

        public List<IpLookupDataViewModel> GetReasonsList()
        {
            return new List<IpLookupDataViewModel>
                {
                    new IpLookupDataViewModel
                    {
                        Id = 1,
                        Name = "Learner's medical reason",
                        IsSelected = true
                    },
                    new IpLookupDataViewModel
                    {
                        Id = 2,
                        Name = "Domestic crisis",
                        IsSelected = false
                    }
                };
        }
    }
}