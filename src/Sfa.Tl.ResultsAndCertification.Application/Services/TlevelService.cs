﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TlevelService : ITlevelService
    {
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;
        private readonly IRepository<TqAwardingOrganisation> _awardingOrganisationRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public TlevelService(
            ResultsAndCertificationConfiguration resultsAndCertificationConfiguration,
            IRepository<TqAwardingOrganisation> _repository,
            INotificationService notificationService,
            IMapper mapper)
        {
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
            _awardingOrganisationRepository = _repository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            var tlevels = await _awardingOrganisationRepository
                .GetManyAsync(x => x.TlAwardingOrganisaton.UkPrn == ukprn,
                        n => n.TlPathway,
                        n => n.TlAwardingOrganisaton)
                .OrderBy(x => x.TlPathway.TlevelTitle)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            var tlevels = await _awardingOrganisationRepository
               .GetManyAsync(x => x.IsActive && x.TlAwardingOrganisaton.UkPrn == ukprn && statusId == x.ReviewStatus && x.TlPathway.IsActive,
                       n => n.TlPathway,
                       n => n.TlAwardingOrganisaton)
               .ToListAsync();

            return _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
        }

        public async Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model)
        {
            var tqAwardingOrganisation = await _awardingOrganisationRepository
                .GetSingleOrDefaultAsync(p => p.Id == model.TqAwardingOrganisationId,
                        navigationPropertyPath: new Expression<Func<TqAwardingOrganisation, object>>[]
                        {
                            p => p.TlPathway
                        });

            if (tqAwardingOrganisation == null) return false;

            tqAwardingOrganisation = _mapper.Map(model, tqAwardingOrganisation);

            if (model.PathwayStatusId == (int)TlevelReviewStatus.Queried && !string.IsNullOrWhiteSpace(model.Query))
            {
                var referenceNumber = Guid.NewGuid().ToString();
                var hasTechTeamEmailSent = await SendEmailAsync(model, tqAwardingOrganisation, referenceNumber);

                var userTokens = new Dictionary<string, dynamic> { { "reference_number", referenceNumber } };
                var hasUserEmailSent = await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.TlevelDetailsQueriedUserNotification.ToString(), model.QueriedUserEmail, userTokens);

                if (!hasTechTeamEmailSent || !hasUserEmailSent) return false;
            }

            return await _awardingOrganisationRepository.UpdateAsync(tqAwardingOrganisation) > 0;
        }

        private async Task<bool> SendEmailAsync(VerifyTlevelDetails model, TqAwardingOrganisation tqAwardingOrganisation, string referenceNumber)
        {
            var tokens = new Dictionary<string, dynamic>
            {
                { "reference_number", referenceNumber },
                { "sender_email_address", model.QueriedUserEmail },
                { "tqawardingorganisation_id", tqAwardingOrganisation.Id },
                { "tlevel_name", tqAwardingOrganisation.TlPathway.TlevelTitle },
                { "requested_message", model.Query }
            };

            return await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.TlevelDetailsQueriedTechnicalTeamNotification.ToString(), _resultsAndCertificationConfiguration.TlevelQueriedSupportEmailAddress, tokens);
        }
    }
}