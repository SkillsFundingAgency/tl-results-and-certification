using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult ActualResult { get; set; }
        protected AdminReviewChangesIndustryPlacementViewModel ViewModel;


        public async override Task When()
        {
            ActualResult = await Controller.AdminReviewChangesIndustryPlacementAsync(ViewModel);
        }

        protected AdminReviewChangesIndustryPlacementViewModel CreateViewModel(IndustryPlacementStatus? industryPlacementStatus)
        {
            return new AdminReviewChangesIndustryPlacementViewModel
            {
                ChangeReason = "Test Reason",
                ContactName = "Test User",
                ZendeskId = "1234567890",
                LoggedInUser = "System",
                AdminChangeIpViewModel = new AdminChangeIpViewModel()
                {
                    AdminIpCompletion = new AdminIpCompletionViewModel()
                    {
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatusTo = industryPlacementStatus

                    }

                }
            };
        }

    }
}
