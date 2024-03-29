﻿namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataHeader
    {
        public char UcasRecordType { get; set; }
        public string SendingOrganisation { get; set; }
        public string ReceivingOrganisation { get; set; }
        public char UcasDataType { get; set; }
        public string ExamMonth { get; set; }
        public string ExamYear { get; set; }
        public string DateCreated { get; set; }
        public string RecordTerminator { get { return string.Empty; } }
    }
}