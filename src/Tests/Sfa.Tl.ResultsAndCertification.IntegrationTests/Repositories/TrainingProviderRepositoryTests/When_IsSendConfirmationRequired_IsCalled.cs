using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.TrainingProviderRepositoryTests
{
    public class When_IsSendConfirmationRequired_IsCalled : TrainingProviderRepositoryBaseTest
    {
        private bool? _result = null;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedQualificationData();

            var seedUlnQualifications = new List<SeedUlnQualifications>
            {
                new SeedUlnQualifications { Uln = 1111111111, HasSendQualification = false, HasSendGrade = false },
                new SeedUlnQualifications { Uln = 1111111112, HasSendQualification = true, HasSendGrade = false },
                new SeedUlnQualifications { Uln = 1111111113, HasSendQualification = false, HasSendGrade = true },
                new SeedUlnQualifications { Uln = 1111111114, HasSendQualification = true, HasSendGrade = true },
            };
            SeedProfileAndQualification(seedUlnQualifications);
            DbContext.SaveChanges();

            // Test class.
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TraningProviderRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(int profileId)
        {
            if (_result != null)
                return;

            _result = await TrainingProviderRepository.IsSendConfirmationRequiredAsync(profileId);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, bool expectedResponse)
        {
            var profile = DbContext.TqRegistrationProfile.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
            await WhenAsync(profile.Id);

            _result.Should().Be(expectedResponse);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, false },
                    new object[] { 1111111112, true },
                    new object[] { 1111111113, true },
                    new object[] { 1111111114, true },
                };
            }
        }
    }
}
