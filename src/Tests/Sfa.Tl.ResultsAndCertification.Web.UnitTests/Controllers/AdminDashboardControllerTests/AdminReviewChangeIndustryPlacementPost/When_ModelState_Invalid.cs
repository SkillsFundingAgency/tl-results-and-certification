using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementPost
{
    public class When_ModelState_Invalid : TestSetup
    {

        private AdminChangeIpViewModel _cacheModel;
        private const string ErrorKey = "AdminReviewChangesIndustryPlacement";

        public override void Given()
        {
          
            ViewModel = CreateViewModel(IndustryPlacementStatus.CompletedWithSpecialConsideration);
            _cacheModel = new AdminChangeIpViewModel()
            {
                AdminIpCompletion = new AdminIpCompletionViewModel()
                {
                    RegistrationPathwayId = 1,
                    IndustryPlacementStatusTo = IndustryPlacementStatus.CompletedWithSpecialConsideration
                    

                },  
                ReasonsViewModel = new AdminIpSpecialConsiderationReasonsViewModel()
                {
                    ReasonsList = new List<IpLookupDataViewModel>
                    {
                        new IpLookupDataViewModel
                        {
                            Id = 1,
                            Name = "Domestic crisis",
                            IsSelected = false
                        }
                    }
                },
                 HoursViewModel = new AdminIpSpecialConsiderationHoursViewModel
                {
                    RegistrationPathwayId = 1,
                    Hours = "10"
                }

            };

           Controller.ModelState.AddModelError(ErrorKey, ReviewChangeAssessment.Validation_Contact_Name_Blank_Text);
           CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cacheModel);

        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var model = ActualResult.ShouldBeViewResult<AdminReviewChangesIndustryPlacementViewModel>();

            model.Should().NotBeNull();
            model.AdminChangeIpViewModel.ReasonsViewModel.Should().NotBeNull();
            model.AdminChangeIpViewModel.HoursViewModel.Should().NotBeNull();
        }

    }
}
