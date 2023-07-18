using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using IndustryPlacementConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement.IndustryPlacementStatusConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusConverter
{
    public abstract class TestSetup : ConverterBaseTest<IndustryPlacementConverter, TqRegistrationPathway, IndustryPlacementStatus>
    {
    }
}