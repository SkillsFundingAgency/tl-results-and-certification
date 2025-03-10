using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests
{
    public abstract class AwardingOrganisationServiceBaseTest : BaseTest<TlAwardingOrganisation>
    {
        protected AwardingOrganisationService CreateService()
            => new(Repository, CreateMapper());

        private static Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AwardingOrganisationMapper).Assembly));
            return new Mapper(mapperConfig);
        }

        protected AwardingOrganisationService Service;

        protected TlAwardingOrganisation Ncfe = new()
        {
            Id = 1,
            UkPrn = 10009696,
            DisplayName = "Ncfe",
            Name = "Ncfe"
        };

        protected TlAwardingOrganisation Pearson = new()
        {
            Id = 2,
            UkPrn = 10011881,
            DisplayName = "Pearson",
            Name = "Pearson"
        };

        public override void Given()
        {
            DbContext.AddRange(new[] { Ncfe, Pearson });
            DbContext.SaveChanges();

            Service = CreateService();
        }
    }
}