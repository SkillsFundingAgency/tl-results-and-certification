using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminChangeLogServiceTests
{
    public class When_GetChangeLogRecordAsync_IsCalled : AdminChangeLogServiceBaseTest
    {
        ChangeLog _expectedResult;
        AdminChangeLogRecord _actualResult;

        public override void Given()
        {
            TqRegistrationPathway _tqRegistrationPathway = new()
            {
                Id = 1,
                TqRegistrationProfileId = 1,
                TqRegistrationProfile = new()
                {
                    Id = 1,
                    UniqueLearnerNumber = 1234567890,
                    Firstname = "John",
                    Lastname = "Wayne"
                },
                TqPathwayAssessments = new List<TqPathwayAssessment>()
                {
                    new TqPathwayAssessment() {
                        AssessmentSeries = new AssessmentSeries(){
                            Id = 1,
                            ComponentType = ComponentType.Core,
                            Name="Summer 2023"
                        },
                        IsOptedin = true,
                        TqRegistrationPathwayId = 1,
                        AssessmentSeriesId = 1
                    }
                },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>()
                {
                    new TqRegistrationSpecialism()
                    {
                        TqSpecialismAssessments = new List<TqSpecialismAssessment>()
                        {
                            new TqSpecialismAssessment()
                            {
                                TqSpecialismResults = new List<TqSpecialismResult>()
                                {
                                    new TqSpecialismResult()
                                    {
                                        Id = 1,
                                        TqSpecialismAssessmentId = 1,
                                        TlLookupId = 1,
                                    }
                                },
                                AssessmentSeries = new AssessmentSeries()
                                {
                                     Id = 1,
                                     ComponentType = ComponentType.Specialism,
                                     Name = "Summer 2024"
                                }
                            }
                        }
                    }
                }
            };

            _expectedResult = new ChangeLog()
            {
                TqRegistrationPathwayId = 1,
                ChangeType = ChangeType.StartYear,
                Details = "{\"StartYearFrom\":2023,\"StartYearTo\":2021}",
                Name = "Martin Guptill",
                DateOfRequest = new DateTime(2024, 01, 01),
                ReasonForChange = "Change reason details",
                ZendeskTicketID = "1234567890",
                TqRegistrationPathway = _tqRegistrationPathway
            };

            AdminChangeLogRepository.GetChangeLogRecordAsync(Arg.Any<int>()).Returns(_expectedResult);
        }

        public override async Task When()
        {
            _actualResult = await AdminChangeLogService.GetChangeLogRecordAsync(_expectedResult.Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
        }
    }
}