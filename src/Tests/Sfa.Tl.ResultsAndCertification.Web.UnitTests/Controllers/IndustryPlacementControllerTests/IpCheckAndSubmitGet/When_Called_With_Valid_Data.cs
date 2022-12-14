using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IndustryPlacementViewModel _cacheModel;
        private IpCheckAndSubmitViewModel _learnerDetails;
        private (List<SummaryItemModel>, bool) _summaryDetailsList;

        public override void Given()
        {
            // Cache object
            _cacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { ProfileId = 1, RegistrationPathwayId = 1, PathwayId = 11, AcademicYear = 2020, IndustryPlacementStatus = IndustryPlacementStatus.Completed },
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheModel);

            // LearnerDetails
            _learnerDetails = new IpCheckAndSubmitViewModel { ProfileId = 1, Uln = 1234567890, LearnerName = "John Smith", DateofBirth = DateTime.Today.AddYears(-18) };
            IndustryPlacementLoader.GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(ProviderUkprn, _cacheModel.IpCompletion.ProfileId).Returns(_learnerDetails);

            // SummaryDetails 
            _summaryDetailsList = (new List<SummaryItemModel> { new SummaryItemModel { Title = "Title 1", Value = "Value 1" }, new SummaryItemModel { Title = "Title 2", Value = "Value 2" } }, true);
            IndustryPlacementLoader.GetIpSummaryDetailsListAsync(_cacheModel).Returns(_summaryDetailsList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(ProviderUkprn, _cacheModel.IpCompletion.ProfileId);
            IndustryPlacementLoader.Received(1).GetIpSummaryDetailsListAsync(_cacheModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpCheckAndSubmitViewModel));

            var model = viewResult.Model as IpCheckAndSubmitViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_learnerDetails.ProfileId);

            // Learner Name
            model.SummaryLearnerName.Title.Should().Be(CheckAndSubmitContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_learnerDetails.LearnerName);

            // Uln
            model.SummaryUln.Title.Should().Be(CheckAndSubmitContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_learnerDetails.Uln.ToString());

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(CheckAndSubmitContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_learnerDetails.DateofBirth.ToDobFormat());

            // Tlevel
            model.SummaryTlevelTitle.Title.Should().Be(CheckAndSubmitContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_learnerDetails.TlevelTitle);

            model.IpDetailsList.Should().BeEquivalentTo(_summaryDetailsList.Item1);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            var expectedBackLink = new BackLinkModel { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, model.ProfileId.ToString() } } };
            model.BackLink.Should().BeEquivalentTo(expectedBackLink);
        }
    }
}
