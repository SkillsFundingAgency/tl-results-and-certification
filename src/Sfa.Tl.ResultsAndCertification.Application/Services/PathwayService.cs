using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PathwayService : IPathwayService
    {
        private readonly IMapper _mapper;

        public PathwayService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(int id)
        {
            var tLevelDetails = new TlevelPathwayDetails
            {
                RouteId = 1,
                PathwayId = 1,
                RouteName = "Construction",
                PathwayName = "Construction: Design, Surveying and Planning",
                Specialisms = new List<string>
                {
                    "Surveying and design for construction and the built environment",
                    "Civil engineering",
                    "Building services design",
                    "Hazardous materials analysis and surveying"
                }
            };

             // TODO: access repository and get details. 

            var awardOrgPathwayStatus = _mapper.Map<TlevelPathwayDetails>(tLevelDetails);
            return await Task.Run(() => awardOrgPathwayStatus);
        }
    }
}
