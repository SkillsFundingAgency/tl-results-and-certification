using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly ILogger<CommonService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        

        public CommonService(ILogger<CommonService> logger, IMapper mapper, IRepository<TlLookup> tlLookupRepository, IRepository<FunctionLog> functionLogRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _tlLookupRepository = tlLookupRepository;
            _functionLogRepository = functionLogRepository;            
        }

        public async Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory)
        {
            var lookupData = await _tlLookupRepository.GetManyAsync(x => x.IsActive &&
                                                            x.Category == lookupCategory.ToString())
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
    }
}
