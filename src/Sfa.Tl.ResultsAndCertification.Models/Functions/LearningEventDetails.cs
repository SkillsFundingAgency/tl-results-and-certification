namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class LearningEventDetails
    {
        public string QualificationCode { get; set; }
        public string QualificationTitle { get; set; }
        public string Grade { get; set; }
        public bool IsAchieved { get; set; }

        public int QualificationId { get; set; }
        public int QualificationGradeId { get; set; }
        public bool IsQualificationAllowed { get; set; }
        public bool IsEnglishSubject { get; set; }
        public bool IsMathsSubject { get; set; }
    }
}
