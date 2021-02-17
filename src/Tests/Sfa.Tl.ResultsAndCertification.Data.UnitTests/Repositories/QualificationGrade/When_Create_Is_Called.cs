using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.QualificationGrade
{
    public class When_Create_Is_Called : BaseTest<Domain.Models.QualificationGrade>
    {
        private int _result;
        private Domain.Models.QualificationGrade _data;

        public override void Given()
        {
            _data = new QualificationGradeBuilder().Build();

        }
        public async override Task When()
        {
            _result = await Repository.CreateAsync(_data);
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
                _result.Should().Be(1);
    }
}
