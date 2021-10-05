using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract = Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly ILogger<CommonService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly INotificationService _notificationService;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public CommonService(ILogger<CommonService> logger, IMapper mapper,
            IRepository<TlLookup> tlLookupRepository,
            IRepository<FunctionLog> functionLogRepository,
            ICommonRepository commonRepository, INotificationService notificationService,
            ResultsAndCertificationConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _tlLookupRepository = tlLookupRepository;
            _functionLogRepository = functionLogRepository;
            _commonRepository = commonRepository;
            _notificationService = notificationService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory)
        {
            var lookupData = await _tlLookupRepository.GetManyAsync(x => x.IsActive && x.Category == lookupCategory.ToString())
                                                      .OrderBy(x => x.SortOrder).ToListAsync();
            
            return _mapper.Map<IEnumerable<LookupData>>(lookupData);
        }

        public async Task<bool> CreateFunctionLog(FunctionLogDetails model)
        {
            if (model != null)
            {               
                var entityModel = _mapper.Map<FunctionLog>(model);
                var isSuccess = await _functionLogRepository.CreateAsync(entityModel) > 0;

                if(isSuccess)
                {
                    model.Id = entityModel.Id;
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdateFunctionLog(FunctionLogDetails model)
        {
            if (model != null)
            {
                var functionLogEntity = await _functionLogRepository.GetFirstOrDefaultAsync(x => x.Id == model.Id);

                if (functionLogEntity == null)
                {
                    _logger.LogWarning(LogEvent.NoDataFound, $"FunctionLog record not found for Id = {model.Id} and FunctionName = {model.Name}. Method: UpdateFunctionLog()");
                    return false;
                }

                functionLogEntity.Status = model.Status;
                functionLogEntity.EndDate = model.EndDate;
                functionLogEntity.Message = model.Message;
                functionLogEntity.ModifiedOn = DateTime.UtcNow;
                functionLogEntity.ModifiedBy = model.PerformedBy;

                return await _functionLogRepository.UpdateAsync(functionLogEntity) > 0;
            }
            return false;
        }

        public async Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn)
        {
            return await _commonRepository.GetLoggedInUserTypeInfoAsync(ukprn);
        }

        public async Task<bool> SendFunctionJobFailedNotification(string jobName, string errorMessage)
        {
            var tokens = new Dictionary<string, dynamic>
                {
                    { "job_name", jobName },
                    { "error_message", errorMessage },
                    { "sender_name", Constants.FunctionPerformedBy }
                };

            return await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.FunctionJobFailedNotification.ToString(), _configuration.TechnicalInternalNotificationEmailAddress, tokens);
        }

        public async Task<IEnumerable<Contract.AcademicYear>> GetCurrentAcademicYearsAsync()
        {
            return await _commonRepository.GetCurrentAcademicYearsAsync();
        }

        public async Task<IEnumerable<Contract.AcademicYear>> GetAcademicYearsAsync()
        {
            return await _commonRepository.GetAcademicYearsAsync();
        }
    }
}