﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkProcessRequest
    {
        public long AoUkprn { get; set; }
        public string BlobFileName { get; set; }
        public Guid BlobUniqueReference { get; set; }
        public FileType FileType { get; set; }
        public DocumentType DocumentType { get; set; }
        public LoginUserType LoginUserType { get; set; }
        public string PerformedBy { get; set; }
    }
}
