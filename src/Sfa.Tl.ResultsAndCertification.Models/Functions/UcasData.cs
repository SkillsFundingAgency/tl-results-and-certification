using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasData
    {
        public UcasDataHeader Header { get; set; }
        public IEnumerable<UcasDataRecord> UcasDataRecords { get; set; }
        public UcasDataTrailer Trailer { get; set; }
    }

    // TODO: Following types to be 
    public enum UcasRecordType
    {
        Header = 'H', 
        Subject = 'S', 
        Trailer = 'T'
    }

    public enum UcasDataType
    {
        Entries = 'E',
        Results = 'R',
        Amendments = 'A'
    }

    public enum UcasGender
    {
        Male = 'M',
        Female = 'F'
    }
}