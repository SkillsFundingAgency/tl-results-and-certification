using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Builders
{
    public class ProviderBuilder
    {
        private int id { get; set; }
        private long ukPrn { get; set; }
        private string name { get; set; }
        private string displayName { get; set; }
        private bool isTlevelProvider { get; set; }

        public ProviderBuilder WithId(int id)
        {
            this.id = id;
            return this;
        }

        public ProviderBuilder WithUkPrn(long ukPrn)
        {
            this.ukPrn = ukPrn;
            return this;
        }

        public ProviderBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ProviderBuilder WithDisplayName(string displayName)
        {
            this.displayName = displayName;
            return this;
        }

        public ProviderBuilder WithIsTlevelProvider(bool isTlevelProvider)
        {
            this.isTlevelProvider = isTlevelProvider;
            return this;
        }

        public Provider Build()
        {
            return new Provider
            {
                Id = this.id,
                UkPrn = this.ukPrn,
                Name = this.name,
                DisplayName = this.displayName,
                IsTlevelProvider = this.isTlevelProvider
            };
        }
    }
}
