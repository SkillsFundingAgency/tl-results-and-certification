using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminBannerService : IAdminBannerService
    {
        private readonly IAdminBannerRepository _bannerRepository;
        private readonly IRepository<Banner> _repository;
        private readonly IMapper _mapper;

        public AdminBannerService(IAdminBannerRepository bannerRepository, IRepository<Banner> repository, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _repository = repository;
            _mapper = mapper;
        }

        public Task<PagedResponse<SearchBannerDetail>> SearchBannersAsync(AdminSearchBannerRequest request)
            => _bannerRepository.SearchBannersAsync(request);

        public async Task<GetBannerResponse> GetBannerAsync(int bannerId)
        {
            Banner banner = await _repository.GetSingleOrDefaultAsync(b => b.Id == bannerId);
            return _mapper.Map<GetBannerResponse>(banner);
        }

        public async Task<AddBannerResponse> AddBannerAsync(AddBannerRequest request)
        {
            Banner banner = _mapper.Map<Banner>(request);
            bool success = await _repository.CreateAsync(banner) > 0;

            return new AddBannerResponse
            {
                Success = success,
                BannerId = banner.Id
            };
        }

        public async Task<bool> UpdateBannerAsync(UpdateBannerRequest request, Func<DateTime> getNow)
        {
            Banner banner = await _repository.GetSingleOrDefaultAsync(b => b.Id == request.BannerId);

            if (banner == null)
            {
                return false;
            }

            banner.Title = request.Title;
            banner.Content = request.Content;
            banner.Target = request.Target;
            banner.Start = request.Start;
            banner.End = request.End;
            banner.IsOptedin = request.IsActive;
            banner.ModifiedBy = request.ModifiedBy;
            banner.ModifiedOn = getNow();

            return await _repository.UpdateAsync(banner) > 0;
        }
    }
}