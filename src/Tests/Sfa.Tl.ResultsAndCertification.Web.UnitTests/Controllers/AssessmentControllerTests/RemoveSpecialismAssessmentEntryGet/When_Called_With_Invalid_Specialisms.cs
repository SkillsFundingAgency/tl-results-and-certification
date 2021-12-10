using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryGet
{
    public class When_Called_With_Invalid_Specialisms : TestSetup
    {
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                AssessmentSeriesName = "Summer 2022",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel> { new SpecialismAssessmentViewModel { AssessmentId = 11 } }
                    },
                    new SpecialismViewModel
                    {
                        Id = 6,
                        LarId = "ZT2158999",
                        Name = "Specialism Name2",
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>{ new SpecialismAssessmentViewModel { AssessmentId = 12 } }
                    }
                }
            };


            SpecialismAssessmentIds = string.Join(Constants.PipeSeperator, new List<string> { "11", "0" });
            _mockresult.SpecialismAssessmentIds = SpecialismAssessmentIds;
            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ProfileId, SpecialismAssessmentIds).Returns(_mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
