﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests
{
    public class When_MapToBatch_IsCalled : CertificateServiceBaseTest
    {
        private IEnumerable<LearnerResultsPrintingData> _inputData;
        private Batch _actualResult;
        private Batch _expectedResult;

        public override void Given()
        {
            CreateService();
            _inputData = new List<LearnerResultsPrintingData>
                            {
                                new LearnerResultsPrintingData
                                {
                                   TlProvider = new TlProvider { Id = 1, UkPrn = 11111111, DisplayName = "Barsley College", Name = "Barsley College", IsActive = true,
                                       TlProviderAddresses = new List<TlProviderAddress> { new TlProviderAddress { Id = 11, DepartmentName = "Dept", AddressLine1 = "Add1", Postcode = "SN1 5JA", Town= "Swindon", IsActive = true } } },

                                   OverallResults = new List<OverallResult>
                                   {
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 1111,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 1111, AcademicYear=2021 ,TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 1111111111, Firstname = "first11", Lastname = "last11", EnglishStatus = SubjectStatus.Achieved, MathsStatus = SubjectStatus.Achieved } },
                                           CertificateType = PrintCertificateType.Certificate,
                                           Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"60358300\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Distinction",
                                           CreatedOn = "05/10/2020".ParseStringToDateTimeWithFormat()
                                       },
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 1112,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 1112,AcademicYear=2021, TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 1111111112, Firstname = "first12", Lastname = "last12", EnglishStatus = SubjectStatus.Achieved, MathsStatus = SubjectStatus.NotAchieved } },
                                           CertificateType = PrintCertificateType.StatementOfAchievement,
                                           Details = "{\"TlevelTitle\":\"T Level in Health\",\"PathwayName\":\"Health\",\"PathwayLarId\":\"6037066X\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":null,\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Merit",
                                           CreatedOn = "15/04/2021".ParseStringToDateTimeWithFormat(),                                           
                                       }
                                   }
                                },

                                new LearnerResultsPrintingData
                                {
                                   TlProvider = new TlProvider { Id = 2, UkPrn = 22222222, DisplayName = "Walsall College", Name = "Walsall College", IsActive = true,
                                       TlProviderAddresses = new List<TlProviderAddress> { new TlProviderAddress { Id = 22, DepartmentName = "Dept", AddressLine1 = "Add1", Postcode = "SN1 5JA", Town= "Swindon", IsActive = true } } },

                                   OverallResults = new List<OverallResult>
                                   {
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 2221,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 2221, AcademicYear = 2021,TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 2222222221, Firstname = "first21", Lastname = "last21" , EnglishStatus = SubjectStatus.NotSpecified, MathsStatus = SubjectStatus.Achieved} },
                                           CertificateType = PrintCertificateType.Certificate,
                                           Details = "{\"TlevelTitle\":\"T Level in Education and Childcare\",\"PathwayName\":\"Education and Childcare\",\"PathwayLarId\":\"60358294\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed with special consideration\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Pass",
                                           CreatedOn = "03/09/2021".ParseStringToDateTimeWithFormat()
                                       },
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 2222,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 2222, AcademicYear=2021, TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 2222222222, Firstname = "first22", Lastname = "last22", EnglishStatus = SubjectStatus.NotAchieved, MathsStatus = SubjectStatus.NotSpecified } },
                                           CertificateType = PrintCertificateType.StatementOfAchievement,
                                           Details = "{\"TlevelTitle\":\"T Level in Science\",\"PathwayName\":\"Science\",\"PathwayLarId\":\"60369899\",\"PathwayResult\":null,\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}, {\"SpecialismName\":\"Civil Engineering\",\"SpecialismLarId\":\"ZTLOS002\",\"SpecialismResult\":null}],\"IndustryPlacementStatus\":null,\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = null,
                                           CreatedOn = "07/05/2022".ParseStringToDateTimeWithFormat()
                                       }
                                   }
                                }
                            };

            _expectedResult = new Batch
            {
                Type = BatchType.Printing,
                Status = BatchStatus.Created,
                CreatedBy = Constants.FunctionPerformedBy,

                PrintBatchItems = new List<PrintBatchItem>
                {
                    new PrintBatchItem
                    {
                        TlProviderAddressId = 11,
                        CreatedBy = Constants.FunctionPerformedBy,
                        PrintCertificates = new List<PrintCertificate>
                        {
                           new PrintCertificate {
                                                  Uln = 1111111111, LearnerName = "First11 Last11", TqRegistrationPathwayId = 1111, Type = PrintCertificateType.Certificate,
                                                  DisplaySnapshot = null, CreatedBy = Constants.FunctionPerformedBy,
                                                  LearningDetails = "{\"TLevelTitle\":\"Design, Surveying and Planning for Construction\",\"Grade\":\"Distinction\",\"Date\":\"October 2020\",\"Core\":\"Design, Surveying and Planning\",\"CoreGrade\":\"A*\",\"OccupationalSpecialism\":[{\"Specialism\":\"Surveying and design for construction and the built environment\",\"Grade\":\"Distinction\"}],\"IndustryPlacement\":\"Met\",\"MARS\":[],\"StartYear\":\"2021\"}"
                                                },
                           new PrintCertificate {
                                                  Uln = 1111111112, LearnerName = "First12 Last12", TqRegistrationPathwayId = 1112, Type = PrintCertificateType.StatementOfAchievement,
                                                  DisplaySnapshot = null, CreatedBy = Constants.FunctionPerformedBy,
                                                  LearningDetails = "{\"TLevelTitle\":\"Health\",\"Grade\":\"\",\"Date\":\"April 2021\",\"Core\":\"Health\",\"CoreGrade\":\"A*\",\"OccupationalSpecialism\":[{\"Specialism\":\"\",\"Grade\":\"Not completed\"}],\"IndustryPlacement\":\"Not met\",\"MARS\":[],\"StartYear\":\"2021\"}"
                                                },
                        }
                    },
                    new PrintBatchItem
                    {
                        TlProviderAddressId = 22,
                        CreatedBy = Constants.FunctionPerformedBy,
                        PrintCertificates = new List<PrintCertificate>
                        {
                           new PrintCertificate {
                                                   Uln = 2222222221, LearnerName = "First21 Last21", TqRegistrationPathwayId = 2221, Type = PrintCertificateType.Certificate,
                                                   DisplaySnapshot = null, CreatedBy = Constants.FunctionPerformedBy,
                                                   LearningDetails = "{\"TLevelTitle\":\"Education and Childcare\",\"Grade\":\"Pass\",\"Date\":\"September 2021\",\"Core\":\"Education and Childcare\",\"CoreGrade\":\"A*\",\"OccupationalSpecialism\":[{\"Specialism\":\"Surveying and design for construction and the built environment\",\"Grade\":\"Distinction\"}],\"IndustryPlacement\":\"Met\",\"MARS\":[],\"StartYear\":\"2021\"}"
                                                },
                           new PrintCertificate {
                                                    Uln = 2222222222, LearnerName = "First22 Last22", TqRegistrationPathwayId = 2222, Type = PrintCertificateType.StatementOfAchievement,
                                                    DisplaySnapshot = null, CreatedBy = Constants.FunctionPerformedBy,
                                                    LearningDetails = "{\"TLevelTitle\":\"Science\",\"Grade\":\"\",\"Date\":\"May 2022\",\"Core\":\"Science\",\"CoreGrade\":\"Not completed\",\"OccupationalSpecialism\":[{\"Specialism\":\"Surveying and design for construction and the built environment\",\"Grade\":\"Distinction\"},{\"Specialism\":\"Civil Engineering\",\"Grade\":\"Not completed\"}],\"IndustryPlacement\":\"Not met\",\"MARS\":[],\"StartYear\":\"2021\"}"
                                                }
                        }
                    }
                }
            };
        }

        public override async Task When()
        {
            await Task.CompletedTask;
            _actualResult = CertificateService.MapToBatch(_inputData);
        }

        [Fact]
        public void Then_ExpectedResults_Are_Returned()
        {
            _actualResult.Should().NotBeNull();

            // Assert Batch
            _actualResult.Type.Should().Be(_expectedResult.Type);
            _actualResult.Status.Should().Be(BatchStatus.Created);
            _actualResult.CreatedBy.Should().Be(Constants.FunctionPerformedBy);

            // Assert Print BatchItems
            _actualResult.PrintBatchItems.Should().HaveCount(_expectedResult.PrintBatchItems.Count);
            AssertPrintBatcheItems(_actualResult.PrintBatchItems.ToList(), _expectedResult.PrintBatchItems.ToList());
        }

        private void AssertPrintBatcheItems(IList<PrintBatchItem> actualPrintBatch, IList<PrintBatchItem> expectedPrintBatch)
        {
            actualPrintBatch.Should().HaveCount(expectedPrintBatch.Count);
            for (int i = 0; i < expectedPrintBatch.Count; i++)
            {
                actualPrintBatch[i].TlProviderAddressId.Should().Be(expectedPrintBatch[i].TlProviderAddressId);
                actualPrintBatch[i].CreatedBy.Should().Be(expectedPrintBatch[i].CreatedBy);

                // Assert Print Certificates
                AssertPrintCertificates(actualPrintBatch[i].PrintCertificates.ToList(), expectedPrintBatch[i].PrintCertificates.ToList());
            }
        }

        private void AssertPrintCertificates(IList<PrintCertificate> actualPrintCertificates, IList<PrintCertificate> expectedPrintCertificates)
        {
            actualPrintCertificates.Should().HaveCount(expectedPrintCertificates.Count);
            for (int i = 0; i < expectedPrintCertificates.Count; i++)
            {
                actualPrintCertificates[i].Uln.Should().Be(expectedPrintCertificates[i].Uln);
                actualPrintCertificates[i].LearnerName.Should().Be(expectedPrintCertificates[i].LearnerName);
                actualPrintCertificates[i].TqRegistrationPathwayId.Should().Be(expectedPrintCertificates[i].TqRegistrationPathwayId);
                actualPrintCertificates[i].Type.Should().Be(expectedPrintCertificates[i].Type);
                actualPrintCertificates[i].DisplaySnapshot.Should().BeNull();
                actualPrintCertificates[i].LearningDetails.Should().Be(expectedPrintCertificates[i].LearningDetails);
                actualPrintCertificates[i].CreatedBy.Should().Be(Constants.FunctionPerformedBy);
            }
        }
    }
}
