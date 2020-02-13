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
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Api
{
    public class When_GetAllTlevelsByAwardingOrganisationIdAsync_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private IMapper _mapper;
        private ILogger<IRepository<TqAwardingOrganisation>> _logger;
        
        private AwardingOrganisationPathwayStatus _data;
        private long ukprn = 10009696;
        
        private Task<IEnumerable<AwardingOrganisationPathwayStatus>> _result;
        private AwardingOrganisationService _service;

        public override void Given()
        {
            CreateTestData();
            
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();

            _data = new AwardingOrganisationPathwayStatus { PathwayId = 1, PathwayName = "Pathway1", RouteName = "Route1", StatusId = 1 };
            _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(Arg.Any<List<TqAwardingOrganisation>>())
                .Returns(new List<AwardingOrganisationPathwayStatus> { _data });

            _service = new AwardingOrganisationService(Repository, _mapper, _logger);
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

        public override void When()
        {
            _result = _service.GetAllTlevelsByAwardingOrganisationIdAsync(ukprn);
        }

        [Fact]
        public void Then_AutoMapper_Map_Performed()
        {
            _mapper.Received().Map<IEnumerable<AwardingOrganisationPathwayStatus>>(Arg.Any<List<TqAwardingOrganisation>>());
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            var expectedResult = _result.Result.FirstOrDefault();

            expectedResult.PathwayId.Should().Be(_data.PathwayId);
            expectedResult.PathwayName.Should().Be(_data.PathwayName);
            expectedResult.RouteName.Should().Be(_data.RouteName);
            expectedResult.StatusId.Should().Be(_data.StatusId);
        }
    }
}
