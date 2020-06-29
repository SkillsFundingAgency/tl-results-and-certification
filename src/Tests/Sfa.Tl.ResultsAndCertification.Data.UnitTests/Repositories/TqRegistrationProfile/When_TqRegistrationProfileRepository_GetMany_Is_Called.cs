using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationProfile
{
    public class When_TqRegistrationProfileRepository_GetMany_Is_Called : BaseTest<Domain.Models.TqRegistrationProfile>
    {
        private IEnumerable<Domain.Models.TqRegistrationProfile> _result;
        private IList<Domain.Models.TqRegistrationProfile> _data;

        public override void Given()
        {
            _data = new TqRegistrationProfileBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(3);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
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
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
