using Sfa.Tl.ResultsAndCertification.Domain.Models;
using PathwayConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult.PathwayResultConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResult.PathwayResultConverter
{
    public abstract class TestSetup : PathwayResultConverterBaseTest<PathwayConverter, TqPathwayResult>
    {
    }
}