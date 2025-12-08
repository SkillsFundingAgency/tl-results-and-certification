using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminAssessmentSeriesDatesService : IAdminAssessmentSeriesDatesService
    {
        private readonly IRepository<AssessmentSeries> _repository;
        private readonly IMapper _mapper;

        public AdminAssessmentSeriesDatesService(IRepository<AssessmentSeries> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAssessmentSeriesDatesDetailsResponse> GetAssessmentSeriesDateAsync(int assessmentId)
        {
            AssessmentSeries assessmentSeries = await _repository.GetFirstOrDefaultAsync(e => e.Id == assessmentId);
            return _mapper.Map<GetAssessmentSeriesDatesDetailsResponse>(assessmentSeries);
        }

        public async Task<IEnumerable<GetAssessmentSeriesDatesDetailsResponse>> SearchAssessmentSeriesDatesAsync(SearchAssessmentSeriesDatesRequest request)
        {
            Expression<Func<AssessmentSeries, bool>> predicate = SearchPredicate(request);

            var assessmentSeries = await _repository.GetManyAsync(predicate).ToListAsync();

            return _mapper.Map<List<GetAssessmentSeriesDatesDetailsResponse>>(assessmentSeries);
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