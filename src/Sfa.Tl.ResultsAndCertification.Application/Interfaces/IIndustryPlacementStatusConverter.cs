using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IIndustryPlacementStatusConverter : IValueConverter<IEnumerable<IndustryPlacement>, IndustryPlacementStatus>
    {
    }
}