using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PathwayService : IPathwayService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly IMapper _mapper;

        public PathwayService(IRepository<TlPathway> pathwayRepository, IMapper mapper)
        {
            _pathwayRepository = pathwayRepository;
            _mapper = mapper;
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(int id)
        {
            // TODO: change this expression to use FirstOrDefault 
            var tlevel = await _pathwayRepository
               .GetManyAsync(x => x.Id == id, 
               n => n.TlRoute,
               n => n.TlSpecialisms)
               .ToListAsync();

            var tlevelPathwayDetails = _mapper.Map<TlevelPathwayDetails>(tlevel.FirstOrDefault());
            return tlevelPathwayDetails;
        }
    }
}
