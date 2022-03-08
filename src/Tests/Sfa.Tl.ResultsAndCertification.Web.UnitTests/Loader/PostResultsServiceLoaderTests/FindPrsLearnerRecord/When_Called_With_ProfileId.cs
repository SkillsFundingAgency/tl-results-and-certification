using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.FindPrsLearnerRecord
{
    public class When_Called_With_ProfileId : TestSetup
    {
        private Models.Contracts.PostResultsService.FindPrsLearnerRecord _expectedApiResult;
        private IList<PrsAssessment> _pathwayAssessments;
        private IList<PrsAssessment> _specialismAssessments;

        public override void Given()
        {
            ProfileId = 1;
            Uln = null;

            _pathwayAssessments = new List<PrsAssessment>
            {
                new PrsAssessment { AssessmentId = 11, SeriesName = "Summer 2021", HasResult = true },
                new PrsAssessment { AssessmentId = 12, SeriesName = "Autumn 2021", HasResult = true }
            };

            _specialismAssessments = new List<PrsAssessment>
            {
                new PrsAssessment { AssessmentId = 15, SeriesName = "Summer 2021", HasResult = true },
                new PrsAssessment { AssessmentId = 16, SeriesName = "Autumn 2021", HasResult = true }
            };

            _expectedApiResult = new Models.Contracts.PostResultsService.FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 54678945,
                TlevelTitle = "Title",
                Status = RegistrationPathwayStatus.Active,
                PathwayAssessments = _pathwayAssessments,
                SpecialismAssessments = _specialismAssessments
            };
            InternalApiClient.FindPrsLearnerRecordAsync(AoUkprn, Uln, ProfileId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(_expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(_expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.Status.Should().Be(_expectedApiResult.Status);

            ActualResult.HasResults.Should().BeTrue();
            ActualResult.PathwayAssessments.Should().NotBeEmpty();
            ActualResult.PathwayAssessments.Count().Should().Be(_pathwayAssessments.Count());

            for (int i = 0; i < _pathwayAssessments.Count(); i++)
            {
                ActualResult.PathwayAssessments.ElementAt(i).AssessmentId.Should().Be(_pathwayAssessments[i].AssessmentId);
                ActualResult.PathwayAssessments.ElementAt(i).SeriesName.Should().Be(_pathwayAssessments[i].SeriesName);
                ActualResult.PathwayAssessments.ElementAt(i).HasResult.Should().Be(_pathwayAssessments[i].HasResult);
            }

            ActualResult.SpecialismAssessments.Should().NotBeEmpty();
            ActualResult.SpecialismAssessments.Count().Should().Be(_specialismAssessments.Count());

            for (int i = 0; i < _pathwayAssessments.Count(); i++)
            {
                ActualResult.SpecialismAssessments.ElementAt(i).AssessmentId.Should().Be(_specialismAssessments[i].AssessmentId);
                ActualResult.SpecialismAssessments.ElementAt(i).SeriesName.Should().Be(_specialismAssessments[i].SeriesName);
                ActualResult.SpecialismAssessments.ElementAt(i).HasResult.Should().Be(_specialismAssessments[i].HasResult);
            }
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            InternalApiClient.Received(1).FindPrsLearnerRecordAsync(AoUkprn, Uln, ProfileId);
        }
    }
}
