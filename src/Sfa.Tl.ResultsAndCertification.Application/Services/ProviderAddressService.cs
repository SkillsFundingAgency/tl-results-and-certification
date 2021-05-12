using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ProviderAddressService : IProviderAddressService
    {
        private readonly IRepository<TlProviderAddress> _tlProviderAddress;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProviderAddressService(IRepository<TlProviderAddress> tlProviderAddress, IMapper mapper, ILogger<ProviderAddressService> logger)
        {
            _tlProviderAddress = tlProviderAddress;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddAddressAsync(AddAddressRequest request)
        {
            var existingAddress = await _tlProviderAddress.GetFirstOrDefaultAsync(x => x.TlProvider.UkPrn == request.Ukprn && x.IsActive);

            var tlProviderAddresses = new List<TlProviderAddress>();

            if (existingAddress != null)
            {
                existingAddress.IsActive = false;
                existingAddress.ModifiedBy = request.PerformedBy;
                existingAddress.ModifiedOn = DateTime.UtcNow;
                tlProviderAddresses.Add(existingAddress);
            }

            tlProviderAddresses.Add(
                new TlProviderAddress
                {
                    TlProviderId = 1, // TODO
                    DepartmentName = request.DepartmentName,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    Town = request.Town,
                    Postcode = request.Postcode,
                    CreatedBy = request.PerformedBy
                }
                );

            return await _tlProviderAddress.UpdateManyAsync(tlProviderAddresses) > 0;
        }
    }
}
