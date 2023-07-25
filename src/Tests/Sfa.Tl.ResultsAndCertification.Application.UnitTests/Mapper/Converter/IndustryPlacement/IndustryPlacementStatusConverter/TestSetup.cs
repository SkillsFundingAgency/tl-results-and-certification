using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;
using IndustryPlacementConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement.IndustryPlacementStatusConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusConverter
{
    public abstract class TestSetup : ConverterBaseTest<IndustryPlacementConverter, IEnumerable<Domain.Models.IndustryPlacement>, IndustryPlacementStatus>
    {
    }
}