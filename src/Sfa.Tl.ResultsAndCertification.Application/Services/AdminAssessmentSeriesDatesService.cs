using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminAssessmentSeriesDatesService : IAdminAssessmentSeriesDatesService
    {
        private readonly IRepository<AssessmentSeries> _repository;
        private readonly ResultsAndCertificationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AdminAssessmentSeriesDatesService(ResultsAndCertificationDbContext dbContext, IRepository<AssessmentSeries> repository, IMapper mapper)
        {
            _dbContext = dbContext;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAssessmentSeriesDatesDetailsResponse> GetAssessmentSeriesDateAsync(int assessmentId)
        {
            AssessmentSeries assessmentSeries = await _repository.GetFirstOrDefaultAsync(e => e.Id == assessmentId);
            return _mapper.Map<GetAssessmentSeriesDatesDetailsResponse>(assessmentSeries);
        }

        public async Task<PagedResponse<GetAssessmentSeriesDatesDetailsResponse>> SearchAssessmentSeriesDatesAsync(SearchAssessmentSeriesDatesRequest request)
        {
            Expression<Func<AssessmentSeries, bool>> predicate = SearchPredicate(request);

            var query = _dbContext.AssessmentSeries.AsQueryable();

            int totalCount = query.Count();

            if (request.SelectedFilters.Any())
                query = query.Where(predicate);

            int filteredRecordCount = await query.CountAsync();
            var pager = new Pager(filteredRecordCount, request.PageNumber, 10);

            IQueryable<GetAssessmentSeriesDatesDetailsResponse> queryableResponse = query
                .Select(e => new GetAssessmentSeriesDatesDetailsResponse()
                {
                    Id=e.Id,
                    Name=e.Name,
                    ComonentType= e.ComponentType,
                    StartDate=e.StartDate,
                    ResultCalculationYear=e.ResultCalculationYear,
                    EndDate=e.EndDate,
                    RommEndDate=e.RommEndDate,
                    AppealEndDate=e.AppealEndDate,
                    ResultPublishDate=e.ResultPublishDate,
                    PrintAvailableDate=e.PrintAvailableDate
                }).OrderBy(e => e.Id)
                .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            List<GetAssessmentSeriesDatesDetailsResponse> assessments = await queryableResponse.ToListAsync();

            return new PagedResponse<GetAssessmentSeriesDatesDetailsResponse>() { Records = assessments, PagerInfo = pager, TotalRecords = totalCount };
        }

        private Expression<Func<AssessmentSeries, bool>> SearchPredicate(SearchAssessmentSeriesDatesRequest request)
        {
            if (request.SelectedFilters == null || request.SelectedFilters.Count == 0)
            {
                return null;
            }
            return a => request.SelectedFilters.Contains((int)a.ComponentType);
        }
    }
}