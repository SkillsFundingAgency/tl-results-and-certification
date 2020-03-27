using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.VerifyTlevelAsync
{
    public class When_VerifyTlevelAsync_IsCalled_Returns_Failure : AwardingOrganisaionServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation;
        private readonly TlevelReviewStatus _updatedTlevelReviewStatus = TlevelReviewStatus.Confirmed;
        private VerifyTlevelDetails _verifyTlevelDetailsModel;
        private bool _isSuccess;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _verifyTlevelDetailsModel = new VerifyTlevelDetails
            {
                TqAwardingOrganisationId = 0,
                PathwayStatusId = (int)TlevelReviewStatus.Confirmed,
                ModifiedBy = "TestUser"
            };

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(_resultsAndCertificationConfiguration, Repository, null, _mapper, _logger);
        }

        public override void When()
        {
            _isSuccess = _service.VerifyTlevelAsync(_verifyTlevelDetailsModel).Result;
        }

        [Fact]
        public void Then_Record_Is_Not_Saved()
        {
            _isSuccess.Should().BeFalse();
        }

        [Fact]
        public void Then_Record_Does_Not_Exist()
        {
            var result = _service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, (int)_updatedTlevelReviewStatus).Result;
            result.Count().Should().Be(0);
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AwardingOrganisationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<VerifyTlevelDetails, TqAwardingOrganisation>(new DateTimeProvider()) :
                                null);
            });
            _mapper = new Mapper(mapperConfig);
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, _awardingOrganisation);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, _awardingOrganisation, _route);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _route, _pathway, _tlAwardingOrganisation, _tlevelReviewStatus);
            DbContext.SaveChangesAsync();
            DetachEntity<TqAwardingOrganisation>();
        }
    }
}
