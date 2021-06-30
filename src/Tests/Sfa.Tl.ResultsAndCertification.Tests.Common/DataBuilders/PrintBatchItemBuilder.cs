using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class PrintBatchItemBuilder
    {
        public PrintBatchItem Build(Batch batch = null, TlProviderAddress tlProviderAddress = null)
        {

            return new PrintBatchItem
            {
                Batch = batch ?? new BatchBuilder().Build(),
                TlProviderAddress = tlProviderAddress ?? new TlProviderAddressBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<PrintBatchItem> BuildList(Batch batch = null)
        {
            var results = new List<PrintBatchItem>();
            var tlProviderAddresses = new TlProviderAddressBuilder().BuildList();
            batch ??= new BatchBuilder().Build();

            foreach(var providerAddress in tlProviderAddresses)
            {
                results.Add(new PrintBatchItem
                {
                    Batch = batch,
                    TlProviderAddress = providerAddress,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                });
            }
            return results;
        }
    }
}
