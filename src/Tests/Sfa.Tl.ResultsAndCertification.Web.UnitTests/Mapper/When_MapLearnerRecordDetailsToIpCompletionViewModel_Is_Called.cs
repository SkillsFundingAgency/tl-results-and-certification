using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Mapper
{
    public class When_MapLearnerRecordDetailsToIpCompletionViewModel_Is_Called
    {
        private readonly IMapper _mapper;

        public When_MapLearnerRecordDetailsToIpCompletionViewModel_Is_Called()
        {
            MapperConfiguration configuration = new(cfg =>
            {
                cfg.AddProfile<IndustryPlacementMapper>();
            });

            _mapper = configuration.CreateMapper();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Object params { learnerRecordDetails, expectedIpCompletionModel }
                    new object[]
                    {
                        CreateLearnerRecordDetails(1, 1, 1, 2020, "John Smith", IndustryPlacementStatus.Completed),
                        CreateIpCompletionViewModel(1,1,1,2020, 2022, "John Smith", IndustryPlacementStatus.Completed)
                    },
                     new object[]
                    {
                        CreateLearnerRecordDetails(2, 99, 57, 2050, "Kate Moss", IndustryPlacementStatus.NotCompleted),
                        CreateIpCompletionViewModel(2, 99, 57, 2050, 2052, "Kate Moss", IndustryPlacementStatus.NotCompleted)
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(LearnerRecordDetails learnerRecordDetails, IpCompletionViewModel expectedIpCompletionModel)
        {
            IpCompletionViewModel ipCompletionViewModel = _mapper.Map<IpCompletionViewModel>(learnerRecordDetails);

            Assert.Equal(ipCompletionViewModel.ProfileId, expectedIpCompletionModel.ProfileId);
            Assert.Equal(ipCompletionViewModel.RegistrationPathwayId, expectedIpCompletionModel.RegistrationPathwayId);
            Assert.Equal(ipCompletionViewModel.PathwayId, expectedIpCompletionModel.PathwayId);
            Assert.Equal(ipCompletionViewModel.AcademicYear, expectedIpCompletionModel.AcademicYear);
            Assert.Equal(ipCompletionViewModel.CompletionAcademicYear, expectedIpCompletionModel.CompletionAcademicYear);
            Assert.Equal(ipCompletionViewModel.LearnerName, expectedIpCompletionModel.LearnerName);
            Assert.Equal(ipCompletionViewModel.IndustryPlacementStatus, expectedIpCompletionModel.IndustryPlacementStatus);

        }

        private static LearnerRecordDetails CreateLearnerRecordDetails(
            int profileId,
            int registrationPathwayId,
            int tlPathwayId,
            int academicYear,
            string name,
            IndustryPlacementStatus industryPlacementStatus)
        {
            return new LearnerRecordDetails
            {
                ProfileId = profileId,
                RegistrationPathwayId = registrationPathwayId,
                TlPathwayId = tlPathwayId,
                AcademicYear = academicYear,
                Name = name,
                IndustryPlacementStatus = industryPlacementStatus
            };
        }

        private static IpCompletionViewModel CreateIpCompletionViewModel(
            int profileId,
            int registrationPathwayId,
            int pathwayId,
            int academicYear,
            int completionAcademicYear,
            string learnerName,
            IndustryPlacementStatus industryPlacementStatus)
        {
            return new IpCompletionViewModel
            {
                ProfileId = profileId,
                RegistrationPathwayId = registrationPathwayId,
                PathwayId = pathwayId,
                AcademicYear = academicYear,
                CompletionAcademicYear = completionAcademicYear,
                LearnerName = learnerName,
                IndustryPlacementStatus = industryPlacementStatus
            };
        }
    }
}