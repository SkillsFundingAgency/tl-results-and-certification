using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using IndustryPlacementConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter
{
    public abstract class TestSetup : BaseTest<IndustryPlacementConverter>
    {
        protected TqRegistrationPathway TqRegistrationPathway;
        protected string Result;

        protected IndustryPlacementConverter Converter;

        public override void Setup()
        {
            Converter = new IndustryPlacementConverter();
        }
    }
}