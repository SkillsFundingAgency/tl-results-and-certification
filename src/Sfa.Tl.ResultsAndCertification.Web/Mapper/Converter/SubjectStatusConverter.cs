using AutoMapper;
using SubjectStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.SubjectStatus;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper.Converter
{
    public class SubjectStatusConverter : IValueConverter<SubjectStatus?, string>
    {
        public string Convert(SubjectStatus? sourceMember, ResolutionContext context)
            => sourceMember switch
            {
                SubjectStatus.Achieved or SubjectStatus.AchievedByLrs => SubjectStatusContent.Achieved_Display_Text,
                _ => SubjectStatusContent.Not_Achieved_Display_Text,
            };
    }
}