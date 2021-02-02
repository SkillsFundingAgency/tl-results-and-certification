using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IMapper _mapper;

        public CommonService(IRepository<TlLookup> tlLookupRepository, IMapper mapper)
        {
            _tlLookupRepository = tlLookupRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory)
        {
            var lookupData = await _tlLookupRepository.GetManyAsync(x => x.IsActive &&
                                                            x.Category == lookupCategory.ToString())
                                                            .OrderBy(x => x.SortOrder).ToListAsync();
            
            return _mapper.Map<IEnumerable<LookupData>>(lookupData);
        }
    }
}
