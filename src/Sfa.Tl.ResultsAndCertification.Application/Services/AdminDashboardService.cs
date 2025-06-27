﻿using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ISystemProvider _systemProvider;
        private readonly IMapper _mapper;

        public AdminDashboardService(
            IAdminDashboardRepository adminDashboardRepository,
            IRepositoryFactory repositoryFactory,
            ISystemProvider systemProvider,
            IMapper mapper)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _repositoryFactory = repositoryFactory;
            _systemProvider = systemProvider;
            _mapper = mapper;
        }

        public async Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync()
        {
            return new AdminSearchLearnerFilters
            {
                AwardingOrganisations = await _adminDashboardRepository.GetAwardingOrganisationFiltersAsync(),
                AcademicYears = await _adminDashboardRepository.GetAcademicYearFiltersAsync(_systemProvider.UtcToday)
            };
        }

        public Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            return _adminDashboardRepository.SearchLearnerDetailsAsync(request);
        }

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId)
        {
            var tqRegistrationPathway = await _adminDashboardRepository.GetLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<AdminLearnerRecord>(tqRegistrationPathway);
        }

        public Task<IList<int>> GetAllowedChangeAcademicYearsAsync(int learnerAcademicYear, int pathwayStartYear)
            => _adminDashboardRepository.GetAllowedChangeAcademicYearsAsync(() => DateTime.Today, learnerAcademicYear, pathwayStartYear);

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request)
        {
            var tqRegistrationPathwayRepository = _repositoryFactory.GetRepository<TqRegistrationPathway>();

            var pathway = await tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;

            pathway.AcademicYear = request.ChangeStartYearDetails.StartYearTo;
            pathway.ModifiedBy = request.CreatedBy;
            pathway.ModifiedOn = _systemProvider.UtcNow;

            bool updated = await tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(pathway, u => u.AcademicYear, u => u.ModifiedBy, u => u.ModifiedOn) > 0;

            if (updated)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                var changeLog = CreateChangeLog(request, request.ChangeStartYearDetails);

                return await changeLongRepository.CreateAsync(changeLog) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessAddCoreAssessmentAsync(ReviewAddCoreAssessmentRequest request)
        {
            var tqRegistrationPathwayRepository = _repositoryFactory.GetRepository<TqRegistrationPathway>();
            var pathway = await tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;
            var tqAssessmentSeriesRepository = _repositoryFactory.GetRepository<AssessmentSeries>();
            var assessmentSeries = await tqAssessmentSeriesRepository.GetFirstOrDefaultAsync(a => a.Name == request.AddCoreAssessmentDetails.CoreAssessmentTo && a.ComponentType == ComponentType.Core);

            int status;
            DateTime utcNow = _systemProvider.UtcNow;
            var pathwayAssessment = new TqPathwayAssessment
            {
                CreatedBy = request.CreatedBy,
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                AssessmentSeriesId = assessmentSeries.Id,
                IsOptedin = true,
                EndDate = pathway.Status == RegistrationPathwayStatus.Withdrawn ? utcNow : null,
                StartDate = utcNow
            };

            var tqPathwayAssesmentRepository = _repositoryFactory.GetRepository<TqPathwayAssessment>();
            status = await tqPathwayAssesmentRepository.CreateAsync(pathwayAssessment);

            if (status > 0)
            {

                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                var changeLog = CreateChangeLog(request, request.AddCoreAssessmentDetails);

                return await changeLongRepository.CreateAsync(changeLog) > 0;
            }

            return false;

        }

        public async Task<bool> ProcessAddSpecialismAssessmentAsync(ReviewAddSpecialismAssessmentRequest request)
        {
            var tqRegistrationPathwayRepository = _repositoryFactory.GetRepository<TqRegistrationPathway>();
            var pathway = await tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;
            var tqAssessmentSeriesRepository = _repositoryFactory.GetRepository<AssessmentSeries>();
            var assessmentSeries = await tqAssessmentSeriesRepository.GetFirstOrDefaultAsync(a => a.Name == request.AddSpecialismDetails.SpecialismAssessmentTo && a.ComponentType == ComponentType.Specialism);

            int status;
            DateTime utcNow = _systemProvider.UtcNow;

            var specialismAssessment = new TqSpecialismAssessment
            {
                CreatedBy = request.CreatedBy,
                TqRegistrationSpecialismId = request.SpecialismId,
                AssessmentSeriesId = assessmentSeries.Id,
                IsOptedin = true,
                EndDate = pathway.Status == RegistrationPathwayStatus.Withdrawn ? utcNow : null,
                StartDate = utcNow
            };

            var tqPathwaySpecialismAssesmentRepository = _repositoryFactory.GetRepository<TqSpecialismAssessment>();
            status = await tqPathwaySpecialismAssesmentRepository.CreateAsync(specialismAssessment);

            if (status > 0)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                var changeLog = CreateChangeLog(request, request.AddSpecialismDetails);

                return await changeLongRepository.CreateAsync(changeLog) > 0;
            }

            return false;

        }

        public async Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request)
        {
            var industryPlacementRepository = _repositoryFactory.GetRepository<IndustryPlacement>();
            IndustryPlacement industryPlacement = await industryPlacementRepository.GetSingleOrDefaultAsync(p => p.TqRegistrationPathwayId == request.RegistrationPathwayId);

            return industryPlacement == null
                ? await CreateIndustryPlacementAsync(request, industryPlacementRepository)
                : await UpdateIndustryPlacementAsync(request, industryPlacement, industryPlacementRepository);
        }

        private async Task<bool> CreateIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request, IRepository<Domain.Models.IndustryPlacement> industryPlacementRepository)
        {
            var newIndustryPlacement = new Domain.Models.IndustryPlacement
            {
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                Status = request.IndustryPlacementStatus,
                Details = CreateIndustryPlacementDetails(request),
                CreatedBy = request.CreatedBy
            };

            bool industryPlacementCreated = await industryPlacementRepository.CreateAsync(newIndustryPlacement) > 0;
            if (industryPlacementCreated)
            {
                var changeLogDetails = new ChangeIndustryPlacementRequest
                {
                    IndustryPlacementStatusFrom = IndustryPlacementStatus.NotSpecified,
                    IndustryPlacementStatusTo = request.IndustryPlacementStatus,
                    HoursSpentOnPlacementFrom = null,
                    HoursSpentOnPlacementTo = request.HoursSpentOnPlacement,
                    SpecialConsiderationReasonsFrom = null,
                    SpecialConsiderationReasonsTo = request.SpecialConsiderationReasons
                };

                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, changeLogDetails)) > 0;
            }

            return false;
        }

        private async Task<bool> UpdateIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request, Domain.Models.IndustryPlacement existingIndustryPlacement, IRepository<Domain.Models.IndustryPlacement> industryPlacementRepository)
        {
            IndustryPlacementDetails existingIndustryPlacementDetails = !string.IsNullOrEmpty(existingIndustryPlacement.Details)
                    ? JsonConvert.DeserializeObject<IndustryPlacementDetails>(existingIndustryPlacement.Details)
                    : null;

            var changeLogDetails = new ChangeIndustryPlacementRequest
            {
                IndustryPlacementStatusFrom = existingIndustryPlacement.Status,
                IndustryPlacementStatusTo = request.IndustryPlacementStatus,
                HoursSpentOnPlacementFrom = existingIndustryPlacementDetails?.HoursSpentOnPlacement,
                HoursSpentOnPlacementTo = request.HoursSpentOnPlacement,
                SpecialConsiderationReasonsFrom = existingIndustryPlacementDetails?.SpecialConsiderationReasons,
                SpecialConsiderationReasonsTo = request.SpecialConsiderationReasons
            };

            existingIndustryPlacement.Status = request.IndustryPlacementStatus;
            existingIndustryPlacement.Details = CreateIndustryPlacementDetails(request);
            existingIndustryPlacement.ModifiedBy = request.CreatedBy;
            existingIndustryPlacement.ModifiedOn = _systemProvider.UtcNow;

            bool industryPlacementUpdated = await industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(existingIndustryPlacement, u => u.Status, u => u.Details, u => u.ModifiedBy, u => u.ModifiedOn) > 0;
            if (industryPlacementUpdated)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, changeLogDetails)) > 0;
            }

            return false;
        }

        private string CreateIndustryPlacementDetails(ReviewChangeIndustryPlacementRequest request)
        {
            if (request.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                return null;
            }

            var details = new IndustryPlacementDetails
            {
                IndustryPlacementStatus = request.IndustryPlacementStatus.ToString(),
                HoursSpentOnPlacement = request.HoursSpentOnPlacement,
                SpecialConsiderationReasons = !request.SpecialConsiderationReasons.IsNullOrEmpty() ? request.SpecialConsiderationReasons : new List<int?>()
            };

            return JsonConvert.SerializeObject(details);
        }

        public async Task<bool> ProcessRemovePathwayAssessmentEntryAsync(ReviewRemoveCoreAssessmentEntryRequest model)
        {
            var pathwayAssessmentRepository = _repositoryFactory.GetRepository<TqPathwayAssessment>();

            var pathwayAssessment = await pathwayAssessmentRepository.GetFirstOrDefaultAsync(pa => pa.Id == model.AssessmentId && pa.IsOptedin
                                                                                              && !pa.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null));
            if (pathwayAssessment == null) return false;

            DateTime utcNow = _systemProvider.UtcNow;

            pathwayAssessment.IsOptedin = false;
            pathwayAssessment.EndDate = utcNow;
            pathwayAssessment.ModifiedOn = utcNow;
            pathwayAssessment.ModifiedBy = model.CreatedBy;

            var isSuccess = await pathwayAssessmentRepository.UpdateAsync(pathwayAssessment) > 0;

            if (isSuccess)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(model, model.ChangeAssessmentDetails)) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessRemoveSpecialismAssessmentEntryAsync(ReviewRemoveSpecialismAssessmentEntryRequest model)
        {
            var specialismAssessmentRepository = _repositoryFactory.GetRepository<TqSpecialismAssessment>();

            var specialismAssessment = await specialismAssessmentRepository.GetFirstOrDefaultAsync(sa => sa.Id == model.AssessmentId && sa.IsOptedin);
            if (specialismAssessment == null) return false;

            DateTime utcNow = _systemProvider.UtcNow;

            specialismAssessment.IsOptedin = false;
            specialismAssessment.EndDate = utcNow;
            specialismAssessment.ModifiedOn = utcNow;
            specialismAssessment.ModifiedBy = model.CreatedBy;

            var isSuccess = await specialismAssessmentRepository.UpdateAsync(specialismAssessment) > 0;

            if (isSuccess)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(model, model.ChangeSpecialismAssessmentDetails)) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessAdminAddPathwayResultAsync(AddPathwayResultRequest request)
        {
            var pathwayAssessmentRepo = _repositoryFactory.GetRepository<TqPathwayAssessment>();

            TqPathwayAssessment pathwayAssessment = await pathwayAssessmentRepo.GetSingleOrDefaultAsync(p => p.Id == request.PathwayAssessmentId);
            if (pathwayAssessment == null)
            {
                return false;
            }

            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();
            DateTime utcNow = _systemProvider.UtcNow;

            var pathwayResult = new TqPathwayResult
            {
                TqPathwayAssessmentId = request.PathwayAssessmentId,
                TlLookupId = request.SelectedGradeId,
                IsOptedin = true,
                StartDate = utcNow,
                EndDate = pathwayAssessment.EndDate.HasValue ? utcNow : null,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = utcNow
            };

            bool created = await pathwayResultRepo.CreateAsync(pathwayResult) > 0;

            if (created)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, new
                {
                    PathwayResultId = pathwayResult.Id,
                    PathwayAssessmentId = request.PathwayAssessmentId,
                    GradeTo = request.GradeTo
                })) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessAdminAddSpecialismResultAsync(AddSpecialismResultRequest request)
        {
            var specialismAssessmentRepo = _repositoryFactory.GetRepository<TqSpecialismAssessment>();

            TqSpecialismAssessment specialismAssessment = await specialismAssessmentRepo.GetSingleOrDefaultAsync(p => p.Id == request.SpecialismAssessmentId);
            if (specialismAssessment == null)
            {
                return false;
            }

            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();
            DateTime utcNow = _systemProvider.UtcNow;

            var specialismResult = new TqSpecialismResult
            {
                TqSpecialismAssessmentId = request.SpecialismAssessmentId,
                TlLookupId = request.SelectedGradeId,
                IsOptedin = true,
                StartDate = utcNow,
                EndDate = specialismAssessment.EndDate.HasValue ? utcNow : null,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = utcNow
            };

            bool created = await specialismResultRepo.CreateAsync(specialismResult) > 0;

            if (created)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, new
                {
                    SpecialismResultId = specialismResult.Id,
                    GradeTo = request.GradeTo
                })) > 0;
            }

            return false;
        }


        public async Task<bool> ProcessAdminChangePathwayResultAsync(ChangePathwayResultRequest request)
        {
            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();
            TqPathwayResult pathwayResult = await pathwayResultRepo.GetSingleOrDefaultAsync(p => p.Id == request.ChangePathwayDetails.PathwayResultId);

            if (pathwayResult == null)
            {
                return false;
            }
            var hasEnddate = pathwayResult.EndDate.HasValue;
            DateTime utcNow = _systemProvider.UtcNow;
            pathwayResult.ModifiedBy = request.CreatedBy;
            pathwayResult.ModifiedOn = utcNow;
            pathwayResult.EndDate = utcNow;
            pathwayResult.IsOptedin = false;

            PrsStatus prstatus = GetPrsStatus(pathwayResult.PrsStatus);

            var updated = await pathwayResultRepo.UpdateWithSpecifedColumnsOnlyAsync(pathwayResult, u => u.ModifiedBy, u => u.ModifiedOn, u => u.EndDate, u => u.IsOptedin) > 0;

            if (request.SelectedGradeId > 0)
            {
                var createPathwayResult = new TqPathwayResult
                {
                    TqPathwayAssessmentId = request.ChangePathwayDetails.PathwayAssessmentId,
                    TlLookupId = request.SelectedGradeId,
                    StartDate = utcNow,
                    IsOptedin = true,
                    IsBulkUpload = false,
                    CreatedBy = request.CreatedBy,
                    EndDate = hasEnddate ? utcNow : null,
                    PrsStatus = prstatus

                };

                bool created = await pathwayResultRepo.CreateAsync(createPathwayResult) > 0;
                request.ChangePathwayDetails.PathwayResultId = createPathwayResult.Id;
            }


            if (updated)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, request.ChangePathwayDetails)) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessAdminChangeSpecialismResultAsync(ChangeSpecialismResultRequest request)
        {
            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();


            TqSpecialismResult specialismResult = await specialismResultRepo.GetSingleOrDefaultAsync(p => p.Id == request.ChangeSpecialismDetails.SpecialismResultId);
            var hasEnddate = specialismResult.EndDate.HasValue;
            if (specialismResult == null)
            {
                return false;
            }
            DateTime utcNow = _systemProvider.UtcNow;
            specialismResult.ModifiedBy = request.CreatedBy;
            specialismResult.ModifiedOn = utcNow;
            specialismResult.EndDate = utcNow;
            specialismResult.IsOptedin = false;

            PrsStatus prstatus = GetPrsStatus(specialismResult.PrsStatus);

            var updated = await specialismResultRepo.UpdateWithSpecifedColumnsOnlyAsync(specialismResult, u => u.ModifiedBy, u => u.ModifiedOn, u => u.EndDate, u => u.IsOptedin) > 0;

            if (request.SelectedGradeId > 0)
            {
                var createSpecialsimResult = new TqSpecialismResult
                {
                    TqSpecialismAssessmentId = request.ChangeSpecialismDetails.SpecialismAssessmentId,
                    TlLookupId = request.SelectedGradeId,
                    StartDate = utcNow,
                    IsOptedin = true,
                    IsBulkUpload = false,
                    CreatedBy = request.CreatedBy,
                    EndDate = hasEnddate ? utcNow : null,
                    PrsStatus = prstatus
                };

                bool created = await specialismResultRepo.CreateAsync(createSpecialsimResult) > 0;
                request.ChangeSpecialismDetails.SpecialismResultId = createSpecialsimResult.Id;
            }


            if (updated)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, request.ChangeSpecialismDetails)) > 0;
            }

            return false;
        }

        public async Task<bool> CreateReplacementDocumentPrintingRequestAsync(ReplacementPrintRequest request)
        {
            IRepository<PrintCertificate> printCertificateRepo = _repositoryFactory.GetRepository<PrintCertificate>();
            IRepository<Batch> batchRepo = _repositoryFactory.GetRepository<Batch>();

            var printCertificate = await printCertificateRepo
                .GetFirstOrDefaultAsync(p => p.Id == request.PrintCertificateId
                                        && p.Uln == request.Uln
                                        && p.PrintBatchItem.TlProviderAddress.TlProvider.UkPrn == request.ProviderUkprn,
                                        navigationPropertyPath: new Expression<Func<PrintCertificate, object>>[]
                                        {
                                            p => p.PrintBatchItem
                                        });

            if (printCertificate == null)
                return false;

            var replacementBatchRequest = _mapper.Map<Batch>(printCertificate, opt =>
            {
                opt.Items["providerAddressId"] = request.ProviderAddressId;
                opt.Items["performedBy"] = request.PerformedBy;
            });

            var result = await batchRepo.CreateAsync(replacementBatchRequest);
            return result > 0;
        }

        public async Task<bool> ProcessChangeMathsStatusAsync(ReviewChangeMathsStatusRequest request)
        {
            var tqRegistrationPathwayRepository = _repositoryFactory.GetRepository<TqRegistrationPathway>();
            var pathway = await tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(
                p => p.Id == request.RegistrationPathwayId,
                navigationPropertyPath: new Expression<Func<TqRegistrationPathway, object>>[]
                {
            p => p.TqRegistrationProfile
                });

            if (pathway == null || pathway.TqRegistrationProfile == null)
                return false;

            var profile = pathway.TqRegistrationProfile;
            var originalMathsStatus = profile.MathsStatus;
            profile.MathsStatus = request.MathsStatusTo;
            profile.ModifiedBy = request.CreatedBy;
            profile.ModifiedOn = _systemProvider.UtcNow;

            var profileRepository = _repositoryFactory.GetRepository<TqRegistrationProfile>();
            bool updated = await profileRepository.UpdateWithSpecifedColumnsOnlyAsync(profile,
                u => u.MathsStatus,
                u => u.ModifiedBy,
                u => u.ModifiedOn) > 0;

            if (updated)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                var changeLog = CreateChangeLog(request, new
                {
                    MathsStatusFrom = originalMathsStatus,
                    request.MathsStatusTo
                });

                return await changeLongRepository.CreateAsync(changeLog) > 0;
            }

            return false;
        }

        private ChangeLog CreateChangeLog(ReviewChangeRequest request, object details)
        {
            const string SystemUser = "System";

            var changeLog = new ChangeLog
            {
                ChangeType = request.ChangeType,
                ReasonForChange = request.ChangeReason,
                DateOfRequest = request.RequestDate,
                Details = JsonConvert.SerializeObject(details),
                ZendeskTicketID = request.ZendeskId,
                Name = request.ContactName,
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                CreatedBy = string.IsNullOrEmpty(request.CreatedBy) ? SystemUser : request.CreatedBy
            };

            return changeLog;
        }

        private static PrsStatus GetPrsStatus(PrsStatus? prsStatus) => prsStatus != PrsStatus.Final ? default : PrsStatus.Final;
    }
}