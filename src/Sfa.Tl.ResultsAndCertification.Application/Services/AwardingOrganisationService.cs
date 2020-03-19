using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AwardingOrganisationService : IAwardingOrganisationService
    {
        private readonly IRepository<TqAwardingOrganisation> _awardingOrganisationRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqAwardingOrganisation>> _logger;

        public AwardingOrganisationService(
            IRepository<TqAwardingOrganisation> _repository,
            INotificationService notificationService,
            IMapper mapper,
            ILogger<IRepository<TqAwardingOrganisation>> logger)
        {
            _awardingOrganisationRepository = _repository;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            var tlevels = await _awardingOrganisationRepository
                .GetManyAsync(x => x.TlAwardingOrganisaton.UkPrn == ukprn,
                        n => n.TlRoute,
                        n => n.TlPathway,
                        n => n.TlAwardingOrganisaton)
                .ToListAsync();

            var awardOrgPathwayStatus = _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
            return awardOrgPathwayStatus;
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            var tlevels = await _awardingOrganisationRepository
               .GetManyAsync(x => x.TlAwardingOrganisaton.UkPrn == ukprn && statusId == x.ReviewStatus,
                       n => n.TlRoute,
                       n => n.TlPathway,
                       n => n.TlAwardingOrganisaton)
               .ToListAsync();

            var awardOrgPathwayStatus = _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
            return awardOrgPathwayStatus;
        }

        public async Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model)
        {
            var tqAwardingOrganisation = await _awardingOrganisationRepository
                .GetSingleOrDefaultAsync(p => p.Id == model.TqAwardingOrganisationId,
                        navigationPropertyPath: new Expression<Func<TqAwardingOrganisation, object>>[]
                        { r => r.TlRoute,
                          p => p.TlPathway
                        });

            if (tqAwardingOrganisation == null) return false;

            tqAwardingOrganisation = _mapper.Map(model, tqAwardingOrganisation);

            if (model.PathwayStatusId == (int)TlevelReviewStatus.Queried && !string.IsNullOrWhiteSpace(model.Query))
            {
                var tokens = new Dictionary<string, dynamic>
                {
                    { "tlevel_name", $"{tqAwardingOrganisation.TlRoute.Name} : { tqAwardingOrganisation.TlPathway.Name }" },
                    { "user_comments", model.Query },
                    { "sender_name", model.ModifiedBy },
                    { "sender_email_address", model.QueriedUserEmail }
                };

                var hasEmailSent = await SendEmailAsync("rajesh.gaddam@digital.education.gov.uk", tokens);
                if (!hasEmailSent) return false;
            }
            return await _awardingOrganisationRepository.UpdateAsync(tqAwardingOrganisation) > 0;
        }

        private async Task<bool> SendEmailAsync(string toAddress, IDictionary<string, dynamic> tokens)
        {
            return await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.TlevelDetailsQueried.ToString(), toAddress, tokens);
        }
    }
}
