using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;
using TheAcademicYearConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.AcademicYearConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.AcademicYearConverter
{
    public abstract class TestSetup : ConverterBaseTest<TheAcademicYearConverter, int, string>
    {
    }
}