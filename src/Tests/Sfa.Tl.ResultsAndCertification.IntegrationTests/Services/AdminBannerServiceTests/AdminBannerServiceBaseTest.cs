using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests
{
    public abstract class AdminBannerServiceBaseTest : BaseTest<Banner>
    {
        protected IAdminBannerService AdminBannerService => CreateAdminBannerService();

        private IAdminBannerService CreateAdminBannerService()
        {
            var bannerRepository = new AdminBannerRepository(DbContext);
            var logger = new Logger<GenericRepository<Banner>>(new NullLoggerFactory());
            var repository = new GenericRepository<Banner>(logger, DbContext);
            IMapper mapper = CreateMapper();

            return new AdminBannerService(bannerRepository, repository, mapper);
        }

        private static Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminProviderMapper).Assembly));
            return new Mapper(mapperConfig);
        }

        protected IList<Banner> BannersDb = new List<Banner> { FirstBanner };

        protected static Banner FirstBanner
            => new()
            {
                Id = 1,
                Title = "banner-title",
                Content = "banner-content",
                Target = BannerTarget.Both,
                Start = new DateTime(2024, 1, 1),
                End = new DateTime(2024, 1, 31),
                IsOptedin = true,
                CreatedBy = "banner-created-by",
                CreatedOn = new DateTime(2024, 1, 1),
                ModifiedBy = "banner-modified-by",
                ModifiedOn = null
            };

        protected void SeedTestData()
        {
            DbContext.Banner.Add(FirstBanner);
            DbContext.SaveChanges();
        }
    }
}