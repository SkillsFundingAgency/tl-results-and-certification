﻿using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IUcasRepository
    {
        public Task<IList<TqRegistrationPathway>> GetUcasDataRecordsForEntriesAsync();
        public Task<IList<OverallResult>> GetUcasDataRecordsForResultsAsync();
        public Task<IList<OverallResult>> GetUcasDataRecordsForAmendmentsAsync();
        public string GetDualSpecialismLarId(List<string> specialismLarIds);
    }
}
