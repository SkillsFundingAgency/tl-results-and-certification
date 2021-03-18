using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.CheckAndSubmit;
using EnglishAndMathsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordCheckAndSubmitGet
{
    public class When_Cache_Found_With_No_Lrs_Data : TestSetup
    {
        private AddLearnerRecordViewModel _cacheResult;
        private EnterUlnViewModel _ulnViewModel;
        private EnglishAndMathsQuestionViewModel _englishAndMathsViewModel;
        private FindLearnerRecord _learnerRecord;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            _routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
            _learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Barnsley College (123456789)", IsLearnerRegistered = true, IsLearnerRecordAdded = false, HasLrsEnglishAndMaths = false };
            _ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
            _englishAndMathsViewModel = new EnglishAndMathsQuestionViewModel { LearnerName = _learnerRecord.Name, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved };
            IndustryPlacementQuestionViewModel = new IndustryPlacementQuestionViewModel { LearnerName = _learnerRecord.Name, IndustryPlacementStatus = IndustryPlacementStatus.Completed };

            _cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = _learnerRecord,
                Uln = _ulnViewModel,
                EnglishAndMathsQuestion = _englishAndMathsViewModel,
                IndustryPlacementQuestion = IndustryPlacementQuestionViewModel
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(CheckAndSubmitViewModel));

            var model = viewResult.Model as CheckAndSubmitViewModel;
            model.Should().NotBeNull();

            // Summary ULN
            model.SummaryUln.Should().NotBeNull();
            model.SummaryUln.Title.Should().Be(CheckAndSubmitContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_ulnViewModel.EnterUln);
            model.SummaryUln.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryUln.RenderActionColumn.Should().BeTrue();
            model.SummaryUln.ActionText.Should().BeNullOrEmpty();
            model.SummaryUln.RouteName.Should().BeNullOrEmpty();
            model.SummaryUln.RouteAttributes.Should().BeNull();

            // Summary LearnerName
            model.SummaryLearnerName.Should().NotBeNull();
            model.SummaryLearnerName.Title.Should().Be(CheckAndSubmitContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_learnerRecord.Name);
            model.SummaryLearnerName.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryLearnerName.RenderActionColumn.Should().BeTrue();
            model.SummaryLearnerName.ActionText.Should().BeNullOrEmpty();
            model.SummaryLearnerName.RouteName.Should().BeNullOrEmpty();
            model.SummaryLearnerName.RouteAttributes.Should().BeNull();

            // Summary DateofBirth
            model.SummaryDateofBirth.Should().NotBeNull();
            model.SummaryDateofBirth.Title.Should().Be(CheckAndSubmitContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_learnerRecord.DateofBirth.ToShortDateString());
            model.SummaryDateofBirth.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryDateofBirth.RenderActionColumn.Should().BeTrue();
            model.SummaryDateofBirth.ActionText.Should().BeNullOrEmpty();
            model.SummaryDateofBirth.RouteName.Should().BeNullOrEmpty();
            model.SummaryDateofBirth.RouteAttributes.Should().BeNull();

            // Summary Provider
            model.SummaryProvider.Should().NotBeNull();
            model.SummaryProvider.Title.Should().Be(CheckAndSubmitContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_learnerRecord.ProviderName);
            model.SummaryProvider.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryProvider.RenderActionColumn.Should().BeTrue();
            model.SummaryProvider.ActionText.Should().BeNullOrEmpty();
            model.SummaryProvider.RouteName.Should().BeNullOrEmpty();
            model.SummaryProvider.RouteAttributes.Should().BeNull();

            // Summary EnglishAndMathsStatus           
            model.SummaryEnglishAndMathsStatus.Should().NotBeNull();
            model.SummaryEnglishAndMathsStatus.Title.Should().Be(CheckAndSubmitContent.Title_EnglishAndMaths_Status_Text);
            model.SummaryEnglishAndMathsStatus.Value.Should().Be(EnglishAndMathsContent.Achieved_Display_Text);
            model.SummaryEnglishAndMathsStatus.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryEnglishAndMathsStatus.RenderActionColumn.Should().BeTrue();
            model.SummaryEnglishAndMathsStatus.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryEnglishAndMathsStatus.RouteName.Should().BeNullOrEmpty();
            model.SummaryEnglishAndMathsStatus.RouteAttributes.Should().BeNull();            

            // Summary IndustryPlacementStatus
            model.SummaryIndustryPlacementStatus.Should().NotBeNull();
            model.SummaryIndustryPlacementStatus.Title.Should().Be(CheckAndSubmitContent.Title_IP_Status_Text);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(EnumExtensions.GetDisplayName<IndustryPlacementStatus>(_cacheResult.IndustryPlacementQuestion.IndustryPlacementStatus));
            model.SummaryIndustryPlacementStatus.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryIndustryPlacementStatus.RenderActionColumn.Should().BeTrue();
            model.SummaryIndustryPlacementStatus.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryIndustryPlacementStatus.RouteName.Should().Be(RouteConstants.AddIndustryPlacementQuestion);
            model.SummaryIndustryPlacementStatus.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }        
    }
}
