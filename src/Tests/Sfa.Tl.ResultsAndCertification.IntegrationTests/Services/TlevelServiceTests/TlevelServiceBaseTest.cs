using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TlevelServiceTests
{
    public abstract class TlevelServiceBaseTest : BaseTest<TqAwardingOrganisation>
    {
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration = new() { TlevelQueriedSupportEmailAddress = "test@test.com" };
        protected RepositoryFactory RepositoryFactory;
        protected TlevelService Service;

        protected TlRoute _route;
        protected TlPathway _pathway;
        protected TlAwardingOrganisation _tlAwardingOrganisation;
        protected TqAwardingOrganisation _tqAwardingOrganisation;
        protected IEnumerable<AwardingOrganisationPathwayStatus> _result;

        protected void CreateService()
        {
            NotificationService = CreateNotificationService();
            Service = new TlevelService(ResultsAndCertificationConfiguration, Repository, NotificationService, CreateMapper());
        }

        private NotificationService CreateNotificationService()
        {
            IRepository<NotificationTemplate> notificationTemplateRepository = new GenericRepository<NotificationTemplate>(Substitute.For<ILogger<GenericRepository<NotificationTemplate>>>(), DbContext);
            return new NotificationService(notificationTemplateRepository, Substitute.For<IAsyncNotificationClient>(), Substitute.For<ILogger<NotificationService>>());
        }

        private static IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AwardingOrganisationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<VerifyTlevelDetails, TqAwardingOrganisation>(new DateTimeProvider()) :
                                null);
            });

            return new Mapper(mapperConfig);
        }

        protected virtual void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Ncfe);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Ncfe);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Ncfe);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, EnumAwardingOrganisation.Ncfe);
            DbContext.SaveChangesAsync();
        }
    }
}