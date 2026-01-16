using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminAssessmentSeriesDatesLoader : IAdminAssessmentSeriesDatesLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _apiClient;
        private readonly IMapper _mapper;

        public AdminAssessmentSeriesDatesLoader(IResultsAndCertificationInternalApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public AdminAssessmentSeriesDatesCriteriaViewModel LoadFilters()
            => new()
            {
                ActiveFilters = new List<FilterLookupData>
                {
                    new()
                    {
                        Id = (int)ComponentType.Core,
                        Name = ComponentType.Core.ToString(),
                        IsSelected = false
                    },
                    new()
                    {
                        Id = (int)ComponentType.Specialism,
                        Name = ComponentType.Specialism.ToString(),
                        IsSelected = false
                    }
                },
                PageNumber = 1
            };

        public async Task<AdminAssessmentSeriesDetailsViewModel> GetAssessmentSeriesDateViewModel(int assessmentId)
        {
            var assessment = await _apiClient.GetAssessmentSeriesDateAsync(assessmentId);
            return _mapper.Map<AdminAssessmentSeriesDetailsViewModel>(assessment);
        }

        public async Task<AdminAssessmentSeriesDatesViewModel> SearchAssessmentSeriesDatesAsync(AdminAssessmentSeriesDatesCriteriaViewModel criteria)
        {
            SearchAssessmentSeriesDatesRequest searchRequest = _mapper.Map<SearchAssessmentSeriesDatesRequest>(criteria);

            PagedResponse<GetAssessmentSeriesDatesDetailsResponse> assessments = await _apiClient.SearchAssessmentSeriesDatesAsync(searchRequest);

            AdminAssessmentSeriesDatesViewModel result = _mapper.Map<AdminAssessmentSeriesDatesViewModel>(assessments);
            result.SearchCriteria = criteria;

            return result;
        }
    }
}