using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        public Task<bool> CompareAndProcessAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments, IList<TqSpecialismAssessment> specialismAssessments)
        {
            throw new NotImplementedException();
        }
    }
}
