using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class LearnerRecordService : ILearnerRecordService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRegistrationRepository _tqRegistrationRepository;
        private readonly IRepository<Qualification> _qualificationRepository;

        public LearnerRecordService(IMapper mapper, ILogger<ILearnerRecordService> logger,
            IRegistrationRepository tqRegistrationRepository,
            IRepository<Qualification> qualificationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _tqRegistrationRepository = tqRegistrationRepository;
            _qualificationRepository = qualificationRepository;
        }

        public async Task<IList<RegisteredLearnerDetails>> GetPendingVerificationAndLearningEventsLearnersAsync()
        {
            var registrationLearners = await _tqRegistrationRepository.GetManyAsync(r => r.IsLearnerVerified == null || r.IsLearnerVerified.Value == false ||
                                                                     ((r.IsEnglishAndMathsAchieved == null || r.IsEnglishAndMathsAchieved.Value == false) &&
                                                                     (r.IsRcFeed == null || r.IsRcFeed.Value == false))).ToListAsync();

            if (registrationLearners == null) return null;

            return _mapper.Map<IList<RegisteredLearnerDetails>>(registrationLearners);
        }

        public async Task<IList<RegisteredLearnerDetails>> GetPendingGenderLearnersAsync()
        {
            var registrationLearners = await _tqRegistrationRepository.GetManyAsync(r => r.Gender == null).ToListAsync();

            if (registrationLearners == null) return null;

            return _mapper.Map<IList<RegisteredLearnerDetails>>(registrationLearners);
        }

        public async Task<LearnerVerificationAndLearningEventsResponse> ProcessLearnerRecordsAsync(List<LearnerRecordDetails> learnerRecords)
        {
            if (learnerRecords == null || !learnerRecords.Any())
            {
                var message = $"No learners data retrieved from LRS to process learner and learning events. Method: ProcessLearnerRecordsAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LearnerVerificationAndLearningEventsResponse { IsSuccess = true, Message = message };
            }

            var profilesAndQualsToUpdate = new List<TqRegistrationProfile>();
            var qualifications = await GetAllQualifications();
            var registrationProfiles = await GetRegistrationProfilesByIds(learnerRecords.Select(x => x.ProfileId).ToList(), includeQualificationAchieved: true);

            learnerRecords.ForEach(learnerRecord =>
            {
                var registrationProfile = registrationProfiles.FirstOrDefault(p => p.Id == learnerRecord.ProfileId);

                if (IsValidLearner(registrationProfile))
                {
                    ProcessLearningEvents(qualifications, learnerRecord);

                    var modifiedProfile = ProcessProfileAndQualificationsAchieved(learnerRecord, registrationProfile);

                    if (modifiedProfile != null)
                        profilesAndQualsToUpdate.Add(modifiedProfile);
                }
            });

            if (profilesAndQualsToUpdate.Any())
            {
                var isSuccess = await _tqRegistrationRepository.UpdateManyAsync(profilesAndQualsToUpdate) > 0;
                return new LearnerVerificationAndLearningEventsResponse { IsSuccess = isSuccess, LrsCount = learnerRecords.Count(), ModifiedCount = profilesAndQualsToUpdate.Count(), SavedCount = isSuccess ? profilesAndQualsToUpdate.Count() : 0 };
            }
            else
            {
                return new LearnerVerificationAndLearningEventsResponse { IsSuccess = true, LrsCount = learnerRecords.Count(), ModifiedCount = 0, SavedCount = 0 };
            }
        }

        public async Task<LearnerGenderResponse> ProcessLearnerGenderAsync(List<LearnerRecordDetails> learnerRecords)
        {
            if (learnerRecords == null || !learnerRecords.Any())
            {
                var message = $"No learners data retrieved from LRS to process gender information. Method: ProcessLearnerGenderAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LearnerGenderResponse { IsSuccess = true, Message = message };
            }

            var profilesToUpdate = new List<TqRegistrationProfile>();
            var registrationProfiles = await GetRegistrationProfilesByIds(learnerRecords.Select(x => x.ProfileId).ToList());

            learnerRecords.ForEach(learnerRecord =>
            {
                var profileToUpdate = registrationProfiles.FirstOrDefault(p => p.Id == learnerRecord.ProfileId);

                if (profileToUpdate != null && profileToUpdate.Gender == null && learnerRecord.IsLearnerVerified && learnerRecord.Gender != null)
                {
                    profileToUpdate.Gender = learnerRecord.Gender;
                    profileToUpdate.ModifiedOn = DateTime.UtcNow;
                    profileToUpdate.ModifiedBy = learnerRecord.PerformedBy;
                    profilesToUpdate.Add(profileToUpdate);
                }
            });

            if (profilesToUpdate.Any())
            {
                var response = await _tqRegistrationRepository.UpdateManyAsync(profilesToUpdate);
                return new LearnerGenderResponse { IsSuccess = response > 0, LrsCount = learnerRecords.Count(), ModifiedCount = profilesToUpdate.Count(), SavedCount = response };
            }
            else
            {
                return new LearnerGenderResponse { IsSuccess = true, LrsCount = learnerRecords.Count(), ModifiedCount = profilesToUpdate.Count(), SavedCount = 0 };
            }
        }


        private static void ProcessLearningEvents(List<Qualification> qualifications, LearnerRecordDetails learnerRecord)
        {
            if (learnerRecord.IsLearnerVerified == false) return;

            foreach (var learnerEvent in learnerRecord.LearningEventDetails)
            {
                var qualification = qualifications.FirstOrDefault(q => q.IsActive && q.Code.Equals(learnerEvent.QualificationCode, StringComparison.InvariantCultureIgnoreCase));
                var qualificationGrade = qualification?.QualificationType?.QualificationGrades?.FirstOrDefault(g => g.IsActive && g.Grade.Equals(learnerEvent.Grade, StringComparison.InvariantCultureIgnoreCase));

                if (qualification != null && qualificationGrade != null)
                {
                    learnerEvent.IsQualificationAllowed = true;
                    learnerEvent.IsAchieved = qualificationGrade.IsAllowable;
                    learnerEvent.QualificationGradeId = qualificationGrade.Id;
                    learnerEvent.QualificationId = qualification.Id;
                    learnerEvent.IsEnglishSubject = qualification.TlLookup?.Code.Equals("Eng", StringComparison.InvariantCultureIgnoreCase) ?? false;
                    learnerEvent.IsMathsSubject = qualification.TlLookup?.Code.Equals("Math", StringComparison.InvariantCultureIgnoreCase) ?? false;
                }
            }
        }

        private static TqRegistrationProfile ProcessProfileAndQualificationsAchieved(LearnerRecordDetails learnerRecord, TqRegistrationProfile profile)
        {
            if (learnerRecord.IsLearnerVerified == false && learnerRecord.IsLearnerVerified == profile.IsLearnerVerified)
                return null;

            var isProfileChanged = false;
            var learnerLearningEvents = learnerRecord.LearningEventDetails.Where(x => x.IsQualificationAllowed);

            if (learnerRecord.IsLearnerVerified != profile.IsLearnerVerified)
            {
                profile.IsLearnerVerified = learnerRecord.IsLearnerVerified;
                profile.ModifiedOn = DateTime.UtcNow;
                profile.ModifiedBy = learnerRecord.PerformedBy;
                isProfileChanged = true;
            }

            if (!profile.IsEnglishAndMathsAchieved.HasValue || !profile.IsEnglishAndMathsAchieved.Value)
            {
                if (learnerLearningEvents.Any())
                {
                    var isQualificationAchievedChanged = false;

                    foreach (var learnerLearningEvent in learnerLearningEvents)
                    {
                        var existingQualificationAchieved = profile.QualificationAchieved.FirstOrDefault(q => q.QualificationId == learnerLearningEvent.QualificationId);

                        if (existingQualificationAchieved != null)
                        {
                            if (existingQualificationAchieved.QualificationGradeId != learnerLearningEvent.QualificationGradeId || existingQualificationAchieved.IsAchieved != learnerLearningEvent.IsAchieved)
                            {
                                existingQualificationAchieved.QualificationGradeId = learnerLearningEvent.QualificationGradeId;
                                existingQualificationAchieved.IsAchieved = learnerLearningEvent.IsAchieved;
                                existingQualificationAchieved.ModifiedBy = learnerRecord.PerformedBy;
                                existingQualificationAchieved.ModifiedOn = DateTime.UtcNow;
                                isQualificationAchievedChanged = true;
                            }
                        }
                        else
                        {
                            profile.QualificationAchieved.Add(new QualificationAchieved
                            {
                                TqRegistrationProfileId = profile.Id,
                                QualificationId = learnerLearningEvent.QualificationId,
                                QualificationGradeId = learnerLearningEvent.QualificationGradeId,
                                IsAchieved = learnerLearningEvent.IsAchieved,
                                CreatedBy = learnerRecord.PerformedBy
                            });
                            isQualificationAchievedChanged = true;
                        }
                    }

                    if (isQualificationAchievedChanged)
                    {
                        profile.IsEnglishAndMathsAchieved = learnerLearningEvents.Any(e => e.IsAchieved && e.IsEnglishSubject) && learnerLearningEvents.Any(e => e.IsAchieved && e.IsMathsSubject);
                        profile.IsRcFeed = false;
                        profile.ModifiedOn = DateTime.UtcNow;
                        profile.ModifiedBy = learnerRecord.PerformedBy;
                        isProfileChanged = true;
                    }
                }
            }

            return isProfileChanged ? profile : null;
        }

        private bool IsValidLearner(TqRegistrationProfile profile)
        {
            return profile != null && (profile.IsRcFeed == null || profile.IsRcFeed.Value == false);
        }
        
        private async Task<List<TqRegistrationProfile>> GetRegistrationProfilesByIds(List<int> profileIds, bool includeQualificationAchieved = false)
        {
            var registrationQueryable = _tqRegistrationRepository.GetManyAsync(p => profileIds.Contains(p.Id));

            if (includeQualificationAchieved)
                registrationQueryable.Include(p => p.QualificationAchieved);

            return await registrationQueryable.ToListAsync();
        }

        private async Task<List<Qualification>> GetAllQualifications()
        {
            return await _qualificationRepository.GetManyAsync(x => x.QualificationType.IsActive && x.IsActive, x => x.QualificationType, x => x.QualificationType.QualificationGrades, x => x.TlLookup).ToListAsync();
        }
    }
}
