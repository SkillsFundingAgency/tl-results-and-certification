using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class WithdrawlService : IWithdrawlService
    {
        private readonly IProviderRepository _tqProviderRepository;
        private readonly IRegistrationRepository _tqRegistrationRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<TqRegistrationSpecialism> _tqRegistrationSpecialismRepository;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;
        private readonly ILogger<IRegistrationRepository> _logger;

        public WithdrawlService(IProviderRepository providerRespository,
            IRegistrationRepository tqRegistrationRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<TqRegistrationSpecialism> tqRegistrationSpecialismRepository,
            ICommonService commonService,
            IMapper mapper,
            ILogger<IRegistrationRepository> logger
            )
        {
            _tqProviderRepository = providerRespository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _tqRegistrationSpecialismRepository = tqRegistrationSpecialismRepository;
            _commonService = commonService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<WithdrawlRecordResponse>> ValidateWithdrawlLearnersAsync(long aoUkprn, IEnumerable<WithdrawlCsvRecordResponse> validWithdrawlsData)
        {
            var response = new List<WithdrawlRecordResponse>();
            int rowNum = 1;

            var registrationProfiles = await _tqRegistrationRepository.GetRegistrationProfilesAsync(validWithdrawlsData.Select(e => new TqRegistrationProfile()
            {
                UniqueLearnerNumber = e.Uln
            }).ToList());

            foreach (var withdrawlData in registrationProfiles)
            {
                rowNum++;

                // Withdrawn Learner
                if (withdrawlData.TqRegistrationPathways.All(e => e.Status != RegistrationPathwayStatus.Active))
                {
                    response.Add(AddStage3ValidationError(rowNum, withdrawlData.UniqueLearnerNumber, ValidationMessages.InactiveUln));
                    continue;
                }

                var pathway = withdrawlData.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active);

                // Pathway Validation
                var pathwaycount = pathway.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null
                                        && a.TqPathwayResults.Any(r => r.IsOptedin && r.EndDate == null
                                        && (r.PrsStatus == PrsStatus.UnderReview || r.PrsStatus == PrsStatus.BeingAppealed))).Count();

                if (pathwaycount > 0)
                {
                    response.Add(AddStage3ValidationError(rowNum, withdrawlData.UniqueLearnerNumber, ValidationMessages.InvalidResultState));
                    continue;
                }

                // Specialism Validation
                var specialisms = pathway.TqRegistrationSpecialisms.Where(s => s.TqSpecialismAssessments.Any(sa => sa.IsOptedin && sa.EndDate == null));

                var specialismcount = specialisms.Where(s => s.TqSpecialismAssessments.Any(sa => sa.TqSpecialismResults.Any(sr => sr.IsOptedin
                                        && sr.EndDate == null
                                        && (sr.PrsStatus == PrsStatus.UnderReview || sr.PrsStatus == PrsStatus.BeingAppealed)))).Count();

                if (specialismcount > 0)
                {
                    response.Add(AddStage3ValidationError(rowNum, withdrawlData.UniqueLearnerNumber, ValidationMessages.InvalidResultState));
                    continue;
                }

                response.Add(new WithdrawlRecordResponse()
                {
                    Uln = withdrawlData.UniqueLearnerNumber,
                    ProfileId = withdrawlData.Id
                });
            }
            return response;
        }

        public async Task<IList<WithdrawlRecordResponse>> ValidateWithdrawlTlevelsAsync(long aoUkprn, IEnumerable<WithdrawlCsvRecordResponse> validWithdrawlsData)
        {
            var response = new List<WithdrawlRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);
            var academicYears = await _commonService.GetAcademicYearsAsync();

            foreach (var withdrawlData in validWithdrawlsData)
            {
                var academicYear = academicYears.FirstOrDefault(x => x.Name.Equals(withdrawlData.AcademicYearName, StringComparison.InvariantCultureIgnoreCase));
                if (academicYear == null)
                {
                    response.Add(AddStage3ValidationError(withdrawlData.RowNum, withdrawlData.Uln, ValidationMessages.AcademicYearIsNotValid));
                    continue;
                }
                else
                    withdrawlData.AcademicYear = academicYear.Year;

                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == withdrawlData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    response.Add(AddStage3ValidationError(withdrawlData.RowNum, withdrawlData.Uln, ValidationMessages.ProviderNotRegisteredWithAo));
                    continue;
                }

                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == withdrawlData.ProviderUkprn && tq.PathwayLarId == withdrawlData.CoreCode);
                if (technicalQualification == null)
                {
                    response.Add(AddStage3ValidationError(withdrawlData.RowNum, withdrawlData.Uln, ValidationMessages.CoreNotRegisteredWithProvider));
                    continue;
                }

                if (withdrawlData.SpecialismCodes.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = withdrawlData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        response.Add(AddStage3ValidationError(withdrawlData.RowNum, withdrawlData.Uln, ValidationMessages.SpecialismNotValidWithCore));
                        continue;
                    }

                    var isSpecialismPartOfCouplets = IsSpecialismPartOfCouplet(technicalQualification.TlSpecialismCombinations, withdrawlData.SpecialismCodes);

                    if (isSpecialismPartOfCouplets)
                    {
                        if (!IsValidCouplet(technicalQualification.TlSpecialismCombinations, withdrawlData.SpecialismCodes))
                        {
                            response.Add(AddStage3ValidationError(withdrawlData.RowNum, withdrawlData.Uln, withdrawlData.SpecialismCodes.Count() == 1 ? ValidationMessages.SpecialismCannotBeSelectedAsSingleOption : ValidationMessages.SpecialismIsNotValid));
                            continue;
                        }
                    }
                }

                response.Add(new WithdrawlRecordResponse
                {
                    Uln = withdrawlData.Uln
                });
            };

            return response;
        }

        public IList<TqRegistrationProfile> TransformWithdrawlModel(IList<WithdrawlRecordResponse> withdrawlsData, string performedBy)
        {
            var registrationProfiles = new List<TqRegistrationProfile>();

            foreach (var (withdrawl, index) in withdrawlsData.Select((value, i) => (value, i)))
            {
                registrationProfiles.Add(new TqRegistrationProfile
                {
                    Id = index - Constants.RegistrationProfileStartIndex,
                    UniqueLearnerNumber = withdrawl.Uln
                });
            }
            return registrationProfiles;
        }

        public async Task<WithdrawlProcessResponse> ProcessWithdrawlsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, string performedBy)
        {
            WithdrawlProcessResponse response = new();
            int processed = 0;
            IList<TqRegistrationPathway> pathways = new List<TqRegistrationPathway>();

            var registrationProfiles = await _tqRegistrationRepository.GetRegistrationProfilesAsync(registrations);

            foreach (var profile in registrationProfiles)
            {
                var registration = await _tqRegistrationRepository.GetRegistrationLiteAsync(AoUkprn, profile.Id, false, includeOverallResults: false);

                if (registration == null || registration.Status != RegistrationPathwayStatus.Active)
                {
                    _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {profile.Id}. Method: WithdrawRegistrationAsync({AoUkprn}, {profile.Id})");
                    response.IsSuccess = false;
                }

                EndRegistrationWithStatus(registration, RegistrationPathwayStatus.Withdrawn, performedBy);
                processed++;
                pathways.Add(registration);
            }

            response.IsSuccess = await _tqRegistrationPathwayRepository.UpdateManyAsync(pathways) > 0;

            if (response.IsValid && response.IsSuccess)
            {
                response.BulkUploadStats = new BulkUploadStats
                {
                    TotalRecordsCount = registrationProfiles.Count,
                    NewRecordsCount = 0,
                    AmendedRecordsCount = processed,
                    UnchangedRecordsCount = registrationProfiles.Count - processed
                };
            }
            return response;
        }

        private async Task<IList<TechnicalQualificationDetails>> GetAllTLevelsByAoUkprnAsync(long ukprn)
        {
            var result = await _tqProviderRepository.GetManyAsync(p => p.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn
                                                                    && p.TqAwardingOrganisation.TlAwardingOrganisaton.IsActive
                                                                    && p.TqAwardingOrganisation.TlPathway.IsActive,
               p => p.TlProvider, p => p.TqAwardingOrganisation, p => p.TqAwardingOrganisation.TlAwardingOrganisaton,
               p => p.TqAwardingOrganisation.TlPathway, p => p.TqAwardingOrganisation.TlPathway.TlPathwaySpecialismCombinations.Where(p => p.IsActive),
               p => p.TqAwardingOrganisation.TlPathway.TlSpecialisms.Where(p => p.IsActive)).ToListAsync();

            return _mapper.Map<IList<TechnicalQualificationDetails>>(result);
        }

        private WithdrawlRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new WithdrawlRecordResponse()
            {
                ValidationErrors = new List<BulkProcessValidationError>()
                {
                    new BulkProcessValidationError
                    {
                        RowNum = rowNum.ToString(),
                        Uln = uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }

        private bool IsSpecialismPartOfCouplet(IEnumerable<KeyValuePair<int, string>> coupletPairs, IEnumerable<string> specialismCodesToRegister)
        {
            if (!coupletPairs.Any() || !specialismCodesToRegister.Any()) return false;

            var coupletSpecialismCodes = new List<string>();

            coupletPairs.Select(x => x.Value).ToList().ForEach(c => { coupletSpecialismCodes.AddRange(c.Split(Constants.PipeSeperator)); });

            var result = specialismCodesToRegister.Any(s => coupletSpecialismCodes.Any(c => c.Equals(s, StringComparison.InvariantCultureIgnoreCase)));
            return result;
        }

        private bool IsValidCouplet(IEnumerable<KeyValuePair<int, string>> coupletPairs, IEnumerable<string> specialismCodesToRegister)
        {
            if (!coupletPairs.Any() || !specialismCodesToRegister.Any()) return false;

            var coupletSpecialismCodes = coupletPairs.Select(x => x.Value).ToList();

            var hasValidSpecialismCodes = coupletSpecialismCodes.Any(cs => specialismCodesToRegister.Except(cs.Split(Constants.PipeSeperator), StringComparer.InvariantCultureIgnoreCase).Count() == 0);
            var hasValidCoupletSpecialismCodes = coupletSpecialismCodes.Any(cs => cs.Split(Constants.PipeSeperator).Except(specialismCodesToRegister, StringComparer.InvariantCultureIgnoreCase).Count() == 0);
            return hasValidSpecialismCodes && hasValidCoupletSpecialismCodes;
        }

        private static void EndRegistrationWithStatus(TqRegistrationPathway pathway, RegistrationPathwayStatus status, string performedBy)
        {
            if (pathway != null)
            {
                // Pathway
                pathway.Status = status;
                pathway.IsPendingWithdrawal = false;
                pathway.EndDate = DateTime.UtcNow;
                pathway.ModifiedBy = performedBy;
                pathway.ModifiedOn = DateTime.UtcNow;

                pathway.TqPathwayAssessments.Where(s => s.IsOptedin && s.EndDate == null).ToList().ForEach(pa =>
                {
                    pa.EndDate = DateTime.UtcNow;
                    pa.ModifiedBy = performedBy;
                    pa.ModifiedOn = DateTime.UtcNow;

                    pa.TqPathwayResults.Where(r => r.IsOptedin && r.EndDate == null).ToList().ForEach(pr =>
                    {
                        pr.EndDate = DateTime.UtcNow;
                        pr.ModifiedBy = performedBy;
                        pr.ModifiedOn = DateTime.UtcNow;
                    });
                });

                // Specialisms
                pathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).ToList().ForEach(s =>
                {
                    s.EndDate = DateTime.UtcNow;
                    s.ModifiedBy = performedBy;
                    s.ModifiedOn = DateTime.UtcNow;

                    s.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null).ToList().ForEach(sa =>
                    {
                        sa.EndDate = DateTime.UtcNow;
                        sa.ModifiedBy = performedBy;
                        sa.ModifiedOn = DateTime.UtcNow;

                        sa.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.EndDate == null).ToList().ForEach(sr =>
                        {
                            sr.EndDate = DateTime.UtcNow;
                            sr.ModifiedBy = performedBy;
                            sr.ModifiedOn = DateTime.UtcNow;
                        });
                    });
                });

                // Overall Results
                var overallResult = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
                if (overallResult != null)
                {
                    overallResult.EndDate = DateTime.UtcNow;
                    overallResult.ModifiedBy = performedBy;
                    overallResult.ModifiedOn = DateTime.UtcNow;
                }
            }
        }

    }
}
