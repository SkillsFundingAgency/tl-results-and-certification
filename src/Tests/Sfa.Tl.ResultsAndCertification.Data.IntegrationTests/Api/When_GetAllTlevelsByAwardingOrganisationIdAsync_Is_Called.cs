using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Api
{
    public class When_GetAllTlevelsByAwardingOrganisationIdAsync_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private IMapper _mapper;
        private ILogger<IRepository<TqAwardingOrganisation>> _logger;
        
        private readonly long ukprn = 10009696;
        private IEnumerable<AwardingOrganisationPathwayStatus> _result;
        private AwardingOrganisationService _service;

        public override void Given()
        {
            CreateTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(Repository, _mapper, _logger);
        }
        
        public override void When()
        {
            _result = _service.GetAllTlevelsByUkprnAsync(ukprn).Result;
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            var expectedResult = _result.FirstOrDefault();

            expectedResult.PathwayName.Should().Be("Design, Surveying and Planning");
            expectedResult.RouteName.Should().Be("Construction");
            expectedResult.StatusId.Should().Be(1);
        }

        private void CreateMapper()
        {
            _mapper = new MapperConfiguration(cfg =>
                                cfg.CreateMap<TqAwardingOrganisation, AwardingOrganisationPathwayStatus>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.TlPathway.Id))
                .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.TlRoute.Name))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TlPathway.Name))
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.ReviewStatus)))
                .CreateMapper();
        }

        private void CreateTestData()
        {
            var pathway = new TlPathwayBuilder().Build();
            DbContext.Add(pathway);
            var route = new TlRouteBuilder().Build();
            DbContext.Add(route);
            var aoStatus = new TqAwardingOrganisationBuilder().Build();
            DbContext.Add(aoStatus);
            var awardingOrgs = new TlAwardingOrganisationBuilder().Build();
            DbContext.Add(awardingOrgs);

            DbContext.SaveChangesAsync();
        }
    }
}
