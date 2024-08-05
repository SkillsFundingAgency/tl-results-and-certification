using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.WithdrawalServiceTests.ValidateWithdrawalLearners
{
    public abstract class WithdrawalServiceBaseTest : TestBase
    {
        protected void ConfigureRegistrationRepository(long uln, TqRegistrationProfile profile)
        {
            RegistrationRepository
                .GetRegistrationProfilesAsync(Arg.Is<List<TqRegistrationProfile>>(l => l.Any(p => p.UniqueLearnerNumber == uln)))
                .Returns(new List<TqRegistrationProfile> { profile });
        }

        protected static TqRegistrationProfile CreateTqRegistrationProfile(
            int id,
            long uln,
            DateTime dob,
            RegistrationPathwayStatus status = RegistrationPathwayStatus.Active,
            PrsStatus pathwayResultStatus = PrsStatus.NotSpecified,
            PrsStatus specialismResultStatus = PrsStatus.NotSpecified)
            => new()
            {
                Id = id,
                UniqueLearnerNumber = uln,
                DateofBirth = dob,
                TqRegistrationPathways = new List<TqRegistrationPathway>
                {
                    new()
                    {
                        Status = status,
                        TqPathwayAssessments = new List<TqPathwayAssessment>
                        {
                            new()
                            {
                                IsOptedin = true,
                                TqPathwayResults = new List<TqPathwayResult>
                                {
                                    new()
                                    {
                                        IsOptedin = true,
                                        PrsStatus = pathwayResultStatus
                                    },
                                    new()
                                    {
                                        IsOptedin = true,
                                        PrsStatus = PrsStatus.NotSpecified,
                                    },
                                    new()
                                    {
                                        IsOptedin = false,
                                        PrsStatus = PrsStatus.UnderReview,
                                    },
                                    new()
                                    {
                                        IsOptedin = false,
                                        PrsStatus = PrsStatus.BeingAppealed,
                                    }
                                }
                            }
                        },
                        TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                        {
                            new()
                            {
                                IsOptedin = true,
                                TqSpecialismAssessments = new List<TqSpecialismAssessment>
                                {
                                    new()
                                    {
                                        IsOptedin = true,
                                        TqSpecialismResults = new List<TqSpecialismResult>
                                        {
                                            new()
                                            {
                                                IsOptedin = true,
                                                PrsStatus = specialismResultStatus
                                            },
                                            new()
                                            {
                                                IsOptedin = true,
                                                PrsStatus = PrsStatus.NotSpecified,
                                            },
                                            new()
                                            {
                                                IsOptedin = false,
                                                PrsStatus = PrsStatus.UnderReview,
                                            },
                                            new()
                                            {
                                                IsOptedin = false,
                                                PrsStatus = PrsStatus.BeingAppealed,
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

        protected static WithdrawalCsvRecordResponse CreateWithdrawalCsvRecordResponse(long uln, DateTime dob)
            => new()
            {
                Uln = uln,
                FirstName = "Peter",
                LastName = "Kay",
                DateOfBirth = dob
            };

        protected Task<IList<WithdrawalRecordResponse>> When(long aoUkprn, WithdrawalCsvRecordResponse record)
            => WithdrawalService.ValidateWithdrawalLearnersAsync(aoUkprn, new[] { record });

        protected static void Then_Returns_Expected_ValidationErrors_Results(IList<WithdrawalRecordResponse> result, int rowNum, long uln, string message)
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].ProfileId.Should().Be(default);
            result[0].Uln.Should().Be(default);

            result[0].ValidationErrors.Should().NotBeNull();
            result[0].ValidationErrors.Should().HaveCount(1);

            result[0].ValidationErrors[0].RowNum.Should().Be(rowNum.ToString());
            result[0].ValidationErrors[0].Uln.Should().Be(uln.ToString());
            result[0].ValidationErrors[0].ErrorMessage.Should().Be(message);
        }
    }
}