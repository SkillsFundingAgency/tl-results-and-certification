using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminProviderService : IAdminProviderService
    {
        private readonly IRepository<TlProvider> _repository;
        private readonly IMapper _mapper;

        public AdminProviderService(IRepository<TlProvider> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetProviderResponse> GetProviderAsync(int providerId)
        {
            TlProvider provider = await _repository.GetSingleOrDefaultAsync(p => p.Id == providerId);
            return _mapper.Map<GetProviderResponse>(provider);
        }

        public async Task<AddProviderResponse> AddProviderAsync(AddProviderRequest addRequest)
        {
            AddProviderResponse response = await ValidateAddProvider(addRequest);

            if (!response.IsRequestValid)
            {
                return response;
            }

            TlProvider provider = _mapper.Map<TlProvider>(addRequest);
            response.Success = await _repository.CreateAsync(provider) > 0;
            response.ProviderId = provider.Id;

            return response;
        }

        public async Task<UpdateProviderResponse> UpdateProviderAsync(UpdateProviderRequest updateRequest, Func<DateTime> getNow)
        {
            UpdateProviderResponse validation = await ValidateUpdateProvider(updateRequest);

            if (!validation.IsRequestValid)
            {
                return validation;
            }

            validation.Success = await UpdateProvider(updateRequest, getNow);
            return validation;
        }

        private async Task<AddProviderResponse> ValidateAddProvider(AddProviderRequest addRequest)
        {
            bool ukprnFound = await _repository.AnyAsync(p => p.UkPrn == addRequest.UkPrn);
            bool nameFound = await _repository.AnyAsync(p => EF.Functions.Like(p.Name.Trim(), addRequest.Name.Trim().ToLower()));
            bool displayNameFound = await _repository.AnyAsync(p => EF.Functions.Like(p.DisplayName.Trim(), addRequest.DisplayName.Trim().ToLower()));

            return new AddProviderResponse
            {
                DuplicatedUkprnFound = ukprnFound,
                DuplicatedNameFound = nameFound,
                DuplicatedDisplayNameFound = displayNameFound
            };
        }

        private async Task<UpdateProviderResponse> ValidateUpdateProvider(UpdateProviderRequest updateRequest)
        {
            bool ukprnFound = await _repository.AnyAsync(p => p.Id != updateRequest.ProviderId && p.UkPrn == updateRequest.UkPrn);
            bool nameFound = await _repository.AnyAsync(p => p.Id != updateRequest.ProviderId && EF.Functions.Like(p.Name.Trim(), updateRequest.Name.Trim().ToLower()));
            bool displayNameFound = await _repository.AnyAsync(p => p.Id != updateRequest.ProviderId && EF.Functions.Like(p.DisplayName.Trim(), updateRequest.DisplayName.Trim().ToLower()));

            return new UpdateProviderResponse
            {
                DuplicatedUkprnFound = ukprnFound,
                DuplicatedNameFound = nameFound,
                DuplicatedDisplayNameFound = displayNameFound
            };
        }

        private async Task<bool> UpdateProvider(UpdateProviderRequest updateRequest, Func<DateTime> getNow)
        {
            TlProvider provider = await _repository.GetSingleOrDefaultAsync(p => p.Id == updateRequest.ProviderId);

            provider.UkPrn = updateRequest.UkPrn;
            provider.Name = updateRequest.Name;
            provider.DisplayName = updateRequest.DisplayName;
            provider.IsActive = updateRequest.IsActive;
            provider.ModifiedBy = updateRequest.ModifiedBy;
            provider.ModifiedOn = getNow();

            return await _repository.UpdateAsync(provider) > 0;
        }
    }
}