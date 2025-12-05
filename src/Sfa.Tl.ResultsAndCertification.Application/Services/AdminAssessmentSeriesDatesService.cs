using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using System.Collections.Generic;
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

        public async Task<List<GetAssessmentSeriesDatesResponse>> GetAssessmentSeriesDatesAsync()
        {
            List<AssessmentSeries> assessmentSeries = await _repository.GetManyAsync().ToListAsync();
            return _mapper.Map<List<GetAssessmentSeriesDatesResponse>>(assessmentSeries);
        }

        //public Task<IEnumerable<GetAssessmentSeriesDatesResponse>> SearchAssessmentSeriesDatesAsync(SearchAssessmentSeriesDatesRequest request)
        //    => _repository.GetManyAsync(x => x.ComponentType == (int)request.ComponentType);
    }
}