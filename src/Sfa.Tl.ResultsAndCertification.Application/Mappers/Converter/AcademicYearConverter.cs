using AutoMapper;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class AcademicYearConverter : IValueConverter<int, string>
    {
        public string Convert(int sourceMember, ResolutionContext context)
        {
            int academicYear = sourceMember;

            return $"{academicYear} to {academicYear + 1}";
        }
    }
}