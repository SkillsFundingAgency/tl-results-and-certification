using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordCheckAndSubmitGet
{
    public class CheckAndSubmitTestDataGenerator : IEnumerable<object[]>
    {
        private static readonly FindLearnerRecord _learnersRecordValid = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Barnsley College (123456789)", IsLearnerRegistered = true, IsLearnerRecordAdded = false, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true };
        private static readonly EnterUlnViewModel _ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
        private static readonly EnglishAndMathsQuestionViewModel _englishAndMathsViewModel = new EnglishAndMathsQuestionViewModel {  EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved };
        private static readonly FindLearnerRecord _learnersRecordInvalidLearnerNotRegistered = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Barnsley College (123456789)", IsLearnerRegistered = false, IsLearnerRecordAdded = false, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true };
        private static readonly FindLearnerRecord _learnersRecordInvalidLearnerRecordAdded = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Barnsley College (123456789)", IsLearnerRegistered = true, IsLearnerRecordAdded = true, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true };
        private static readonly FindLearnerRecord _learnersRecordWithNoLrsData = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Barnsley College (123456789)", IsLearnerRegistered = true, IsLearnerRecordAdded = false, HasLrsEnglishAndMaths = false };
        private static readonly IndustryPlacementQuestionViewModel _industryPlacementQuestionModel = new IndustryPlacementQuestionViewModel { LearnerName = _learnersRecordValid.Name, IndustryPlacementStatus = IndustryPlacementStatus.Completed };

        private readonly List<object[]> _data = new List<object[]>
        {
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel(), RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordValid }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordValid, Uln = _ulnViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordValid, Uln = _ulnViewModel, EnglishAndMathsQuestion = _englishAndMathsViewModel, IndustryPlacementQuestion = _industryPlacementQuestionModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordInvalidLearnerNotRegistered, Uln = _ulnViewModel, IndustryPlacementQuestion = _industryPlacementQuestionModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordInvalidLearnerRecordAdded, Uln = _ulnViewModel, IndustryPlacementQuestion = _industryPlacementQuestionModel }, RouteName = RouteConstants.PageNotFound} },               
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordWithNoLrsData, Uln = _ulnViewModel, EnglishAndMathsQuestion = _englishAndMathsViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { AddLearnerRecordViewModel = new AddLearnerRecordViewModel { LearnerRecord = _learnersRecordWithNoLrsData, Uln = _ulnViewModel, IndustryPlacementQuestion = _industryPlacementQuestionModel }, RouteName = RouteConstants.PageNotFound} }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}