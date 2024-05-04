using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class UcasRepository : IUcasRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;
        private readonly ICommonRepository _commonRepository;

        public UcasRepository(ResultsAndCertificationDbContext dbContext, ICommonRepository commonRepository)
        {
            _dbContext = dbContext;
            _commonRepository = commonRepository;
        }

        public async Task<IList<TqRegistrationPathway>> GetUcasDataRecordsForEntriesAsync()
        {
            var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();
            if (currentAcademicYears == null || !currentAcademicYears.Any())
            {
                throw new ApplicationException("Current Academic years are not found. Method: GetCurrentAcademicYearsAsync()");
            }

            var pathwayQueryable = _dbContext.TqRegistrationPathway
                        .Include(x => x.TqProvider)
                            .ThenInclude(x => x.TqAwardingOrganisation)
                            .ThenInclude(x => x.TlPathway)
                        .Include(x => x.TqRegistrationProfile)
                        .Include(x => x.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                        .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                            .ThenInclude(x => x.TqSpecialismAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                            .Include(x => x.IndustryPlacements)
                        .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                            .ThenInclude(x => x.TlSpecialism)
                            .ThenInclude(x => x.TlDualSpecialismToSpecialisms)
                            .ThenInclude(x => x.DualSpecialism)
                        .Where(x => x.Status == RegistrationPathwayStatus.Active && x.EndDate == null &&
                                    x.AcademicYear == currentAcademicYears.FirstOrDefault().Year - 1)
                        .AsQueryable();

            return await pathwayQueryable.ToListAsync();
        }

        public async Task<IList<OverallResult>> GetUcasDataRecordsForResultsAsync()
        {
            var lastAmendmentsRun = GetLastRunOfJob(FunctionType.UcasTransferAmendments);
            if (lastAmendmentsRun == null)
            {
                var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();
                if (currentAcademicYears == null || !currentAcademicYears.Any())
                    throw new ApplicationException("Current Academic years are not found. Method: GetCurrentAcademicYearsAsync()");

                return await _dbContext.OverallResult
                       .Include(x => x.TqRegistrationPathway)
                       .ThenInclude(x => x.TqRegistrationProfile)
                       .Where(x => x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                   x.TqRegistrationPathway.AcademicYear == currentAcademicYears.FirstOrDefault().Year - 1 &&
                                   x.IsOptedin && x.EndDate == null)
                       .ToListAsync();
            }

            return await GetOverallResultsFrom(lastAmendmentsRun.CreatedOn);
        }

        public async Task<IList<OverallResult>> GetUcasDataRecordsForAmendmentsAsync()
        {
            var lastAmendmentsRun = GetLastRunOfJob(FunctionType.UcasTransferAmendments);
            if (lastAmendmentsRun == null)
            {
                var lastResultRun = GetLastRunOfJob(FunctionType.UcasTransferResults);
                if (lastResultRun == null)
                    throw new ApplicationException($"Function log - last run details are not found for the job: {FunctionType.UcasTransferResults}");

                return await GetOverallResultsFrom(lastResultRun.CreatedOn);
            }

            return await GetOverallResultsFrom(lastAmendmentsRun.CreatedOn);
        }

        private async Task<IList<OverallResult>> GetOverallResultsFrom(DateTime lastJobRunDate)
        {
            return await _dbContext.OverallResult
                .Include(x => x.TqRegistrationPathway)
                .ThenInclude(x => x.TqRegistrationProfile)

                .Where(x => x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                            x.IsOptedin && x.EndDate == null &&
                            x.CreatedOn > lastJobRunDate)
                .ToListAsync();
        }

        private FunctionLog GetLastRunOfJob(FunctionType functionType)
        {
            return _dbContext.FunctionLog
                        .Where(x => x.FunctionType == functionType && x.Status == FunctionStatus.Processed)
                        .OrderByDescending(x => x.CreatedOn)
                        .FirstOrDefault();
        }

        private List<TlDualSpecialismToSpecialism> GetDualSpecialisms()
        {
            return _dbContext.TlDualSpecialismToSpecialism
                .Include(x => x.Specialism)
                .Include(x => x.DualSpecialism).ToList();

        }

        public string GetDualSpecialismLarId(List<string> specialismlarId)
        {
            var dualSpecialisms = GetDualSpecialisms();
            var dualSpecialismLarIdForSpecialism = dualSpecialisms.Where(t => specialismlarId.Contains(t.Specialism.LarId.ToString())).ToList();
            var filteredDualSpecialisms = dualSpecialismLarIdForSpecialism.GroupBy(x => x.TlDualSpecialismId).Where(g => g.Count() > 1).ToList();
            var dualSpecialismCodes = filteredDualSpecialisms.SelectMany(t => t).ToList();

            return dualSpecialisms.Where(t => t.TlDualSpecialismId.Equals(dualSpecialismCodes.FirstOrDefault().TlDualSpecialismId)).FirstOrDefault().DualSpecialism.LarId;
        }
    }
}
