using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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

        public async Task<List<AdminAssessmentSeriesDateDetailsViewModel>> GetAssessmentSeriesDatesAsync()
        {
            var assessments = await _apiClient.GetAssessmentSeriesDatesAsync();
            return _mapper.Map<List<AdminAssessmentSeriesDateDetailsViewModel>>(assessments);
        }

        public Task<AdminAssessmentSeriesDateDetailsViewModel> GetAssessmentSeriesDatesDetailsViewModel(int assessmentId)
        {
            throw new System.NotImplementedException();
        }

        //public Task<List<AdminAssessmentSeriesDateDetailsViewModel>> SearchAssessmentSeriesDatesAsync(AdminAssessmentSeriesDatesCriteriaViewModel criteria)
        //{
        //    SearchAssessmentSeriesDatesRequest searchRequest = _mapper.Map<SearchAssessmentSeriesDatesRequest>(criteria);

        //    PagedResponse<SearchNotificationDetail> assessments = await _apiClient.SearchNotificationsAsync(searchRequest);
        //    return _mapper.Map<List<AdminAssessmentSeriesDateDetailsViewModel>>(assessments);

        //    result.SearchCriteria = criteria;
        //    return result;
        //}
    }
}