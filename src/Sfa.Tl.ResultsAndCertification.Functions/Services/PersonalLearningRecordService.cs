using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class PersonalLearningRecordService : IPersonalLearningRecordService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IPersonalLearningRecordApiClient _personalLearningRecordApiClient;
        private readonly ILearnerRecordService _learnerRecordService;

        public PersonalLearningRecordService(IMapper mapper, ILogger<IPersonalLearningRecordService> logger, IPersonalLearningRecordApiClient personalLearningRecordApiClient, ILearnerRecordService learnerRecordService)
        {
            _mapper = mapper;
            _logger = logger;
            _personalLearningRecordApiClient = personalLearningRecordApiClient;
            _learnerRecordService = learnerRecordService;
        }

        public async Task ProcessLearnerVerificationAndLearningEvents()
        {
            var registeredLearners = await _learnerRecordService.GetValidRegistrationLearners();

            if (registeredLearners == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learners found to process learner verification and learning events. Method: ProcessLearnerVerificationAndLearningEvents()");
                return;
            }

            var learnerRecordDetailsList = new List<LearnerRecordDetails>();

            foreach (var learner in registeredLearners.Skip(2).Take(1))
            {
                var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync(learner.Uln.ToString(), learner.Firstname, learner.Lastname, learner.DateofBirth);

                if (plrResult != null)
                {
                    var learnerRecord = _mapper.Map<LearnerRecordDetails>(plrResult, opt => opt.Items["profileId"] = learner.ProfileId);

                    learnerRecord.IsLearnerVerified = true; //!string.IsNullOrWhiteSpace(plrResult.ResponseCode) && plrResult.ResponseCode == "WSEC0208" ? false : true;

                    learnerRecord.LearningEventDetails = new List<LearningEventDetails>();
                    learnerRecord.LearningEventDetails.Add(new LearningEventDetails { Grade = "Pass", QualificationCode = "100/2510/8" });
                    learnerRecord.LearningEventDetails.Add(new LearningEventDetails { Grade = "Fail", QualificationCode = "100/2508/X" });

                    //learnerRecord.LearningEventDetails.Add(new LearningEventDetails { Grade = "A", QualificationCode = "100/0102/5" });
                    //learnerRecord.LearningEventDetails.Add(new LearningEventDetails { Grade = "B", QualificationCode = "500/2735/9" });

                    learnerRecordDetailsList.Add(learnerRecord);
                }
            }

            // process learner records
            var result = await _learnerRecordService.ProcessLearnerRecords(learnerRecordDetailsList);
        }
    }    
}