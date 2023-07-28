using System.Collections.Generic;
using IndustryPlacementConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter
{
    public abstract class TestSetup : ConverterBaseTest<IndustryPlacementConverter, IEnumerable<Domain.Models.IndustryPlacement>, string>
    {
    }
}