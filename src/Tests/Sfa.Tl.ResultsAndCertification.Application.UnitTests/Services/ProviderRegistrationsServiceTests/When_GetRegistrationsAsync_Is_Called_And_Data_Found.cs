using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.ProviderRegistrationsServiceTests
{
    public class When_GetRegistrationsAsync_Is_Called_And_Data_Found : ProviderRegistrationsServiceBaseTest
    {
        private readonly long _providerUkprn = 1;
        private readonly int _startYear = 2020;
        private readonly Guid _blobUniqueRef = new("f2e1a7c3-6e4b-4b0d-9e8a-8a2c1e0f3b5e");
        private readonly string _requestedBy = "test-user";

        private IList<TqRegistrationPathway> _repositoryResult;
        private DataExportResponse _actualResult;

        public override void Given()
        {
            _repositoryResult = new List<TqRegistrationPathway>
            {
                new()
                {
                    AcademicYear = 2021,
                    TqRegistrationProfile = new TqRegistrationProfile
                    {
                        UniqueLearnerNumber = 12,
                        Firstname = "Jessica",
                        Lastname = "Johnson",
                        DateofBirth = new DateTime(2002, 5, 17),
                        EnglishStatus = SubjectStatus.NotAchieved,
                        MathsStatus = SubjectStatus.Achieved
                    },
                    IndustryPlacements = new List<IndustryPlacement>
                    {
                        new()
                        {
                            Status = IndustryPlacementStatus.NotCompleted
                        }
                    },
                    TqProvider = new TqProvider
                    {
                        TqAwardingOrganisation = new TqAwardingOrganisation
                        {
                            TlPathway = new TlPathway
                            {
                                TlevelTitle = "T Level in Digital Business Services",
                                Name = "Digital Business Services",
                                LarId = "10723456"
                            }
                        }
                    },
                    TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                    {
                        new()
                        {
                            TlSpecialism = new TlSpecialism
                            {
                                Name = "Data Technician",
                                LarId = "ZTLOS009"
                            }
                        }
                    }
                }
            };

            ProviderRegistrationsRepository.GetRegistrationsAsync(_providerUkprn, _startYear).Returns(_repositoryResult);

            BlobStorageService.UploadFromByteArrayAsync(Arg.Is<BlobStorageData>(b =>
                b.ContainerName == DocumentType.DataExports.ToString()
                && b.SourceFilePath == $"{_providerUkprn}/{DataExportType.ProviderRegistrations}"
                && b.BlobFileName == _blobUniqueRef.ToString()
                && !b.FileData.IsNullOrEmpty()
                && b.UserName == _requestedBy))
            .Returns(Task.CompletedTask);
        }

        public override async Task When()
        {
            _actualResult = await ProviderRegistrationsService.GetRegistrationsAsync(_providerUkprn, _startYear, _requestedBy, () => _blobUniqueRef);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.FileSize.Should().BeGreaterThan(0);
            _actualResult.BlobUniqueReference.Should().Be(_blobUniqueRef);
            _actualResult.ComponentType.Should().Be(ComponentType.NotSpecified);
            _actualResult.IsDataFound.Should().BeTrue();
        }
    }
}