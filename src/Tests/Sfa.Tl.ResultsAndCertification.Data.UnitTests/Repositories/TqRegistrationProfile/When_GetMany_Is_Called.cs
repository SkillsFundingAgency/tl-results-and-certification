﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationProfile
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.TqRegistrationProfile>
    {
        private IEnumerable<Domain.Models.TqRegistrationProfile> _result;
        private IList<Domain.Models.TqRegistrationProfile> _data;

        public override void Given()
        {
            _data = new TqRegistrationProfileBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Records_Are_Returned() =>
            _result.Count().Should().Be(_data.Count);

        [Fact]
        public void Then_First_Record_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.UniqueLearnerNumber.Should().Be(testData.UniqueLearnerNumber);
            result.Firstname.Should().Be(testData.Firstname);
            result.Lastname.Should().Be(testData.Lastname);
            result.DateofBirth.Should().Be(testData.DateofBirth);
            result.Gender.Should().Be(testData.Gender);
            result.IsLearnerVerified.Should().Be(testData.IsLearnerVerified);
            result.EnglishStatus.Should().Be(testData.EnglishStatus);
            result.MathsStatus.Should().Be(testData.MathsStatus);
            result.IsEnglishAndMathsAchieved.Should().Be(testData.IsEnglishAndMathsAchieved);
            result.IsSendLearner.Should().Be(testData.IsSendLearner);
            result.IsRcFeed.Should().Be(testData.IsRcFeed);
            result.CreatedBy.Should().Be(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
