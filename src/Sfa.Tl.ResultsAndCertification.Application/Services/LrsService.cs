﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
    public class LrsService : ILrsService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRegistrationRepository _tqRegistrationRepository;
        private readonly IRepository<Qualification> _qualificationRepository;

        public LrsService(IMapper mapper, ILogger<ILrsService> logger,
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
            // Get Learners pending for either 'Verification' or 'Maths status' or 'English status' 
            var registrationLearners = await _tqRegistrationRepository.GetManyAsync(r => r.IsLearnerVerified == null || r.IsLearnerVerified.Value == false ||                                                             //IsLearnerVerifyRequired
                                                                                         r.MathsStatus == null || r.MathsStatus == SubjectStatus.NotSpecified || r.MathsStatus == SubjectStatus.NotAchievedByLrs ||    //IsSubjectStatusUpdateRequired(r.MathsStatus)
                                                                                         r.EnglishStatus == null || r.EnglishStatus == SubjectStatus.NotSpecified || r.EnglishStatus == SubjectStatus.NotAchievedByLrs //IsSubjectStatusUpdateRequired(r.EnglishStatus)
                                                                                   ).ToListAsync();

            if (registrationLearners == null) return null;

            return _mapper.Map<IList<RegisteredLearnerDetails>>(registrationLearners);
        }

        public async Task<IList<RegisteredLearnerDetails>> GetPendingGenderLearnersAsync()
        {
            var registrationLearners = await _tqRegistrationRepository.GetManyAsync(r => r.Gender == null).ToListAsync();

            if (registrationLearners == null) return null;

            return _mapper.Map<IList<RegisteredLearnerDetails>>(registrationLearners);
        }

        public async Task<LrsLearnerVerificationAndLearningEventsResponse> ProcessLearnerRecordsAsync(List<LrsLearnerRecordDetails> learnerRecords)
        {
            if (learnerRecords == null || !learnerRecords.Any())
            {
                var message = $"No learners data retrieved from LRS to process learner and learning events. Method: ProcessLearnerRecordsAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LrsLearnerVerificationAndLearningEventsResponse { IsSuccess = true, Message = message };
            }

            var profilesAndQualsToUpdate = new List<TqRegistrationProfile>();
            var qualifications = await GetAllQualifications();
            var registrationProfiles = await _tqRegistrationRepository.GetRegistrationProfilesByIdsAsync(learnerRecords.Select(x => x.ProfileId).ToHashSet(), includeQualificationAchieved: true);

            learnerRecords.ForEach(learnerRecord =>
            {
                var registrationProfile = registrationProfiles.FirstOrDefault(p => p.Id == learnerRecord.ProfileId);

                // Populate Learner Events into learnerRecord object.
                ProcessLearningEvents(qualifications, learnerRecord);

                // 1. Update TqRegistrationProfile with 'Verification, Maths & Eng Status' and
                // 2. Update QualificationAchievements with LrsAchievements
                var modifiedProfile = ProcessProfileAndQualificationsAchieved(qualifications, learnerRecord, registrationProfile);

                if (modifiedProfile != null)
                    profilesAndQualsToUpdate.Add(modifiedProfile);
            });

            if (profilesAndQualsToUpdate.Any())
            {
                var isSuccess = await _tqRegistrationRepository.UpdateManyAsync(profilesAndQualsToUpdate) > 0;
                return new LrsLearnerVerificationAndLearningEventsResponse { IsSuccess = isSuccess, LrsCount = learnerRecords.Count(), ModifiedCount = profilesAndQualsToUpdate.Count(), SavedCount = isSuccess ? profilesAndQualsToUpdate.Count() : 0 };
            }
            else
            {
                return new LrsLearnerVerificationAndLearningEventsResponse { IsSuccess = true, LrsCount = learnerRecords.Count(), ModifiedCount = 0, SavedCount = 0 };
            }
        }

        public async Task<LrsLearnerGenderResponse> ProcessLearnerGenderAsync(List<LrsLearnerRecordDetails> learnerRecords)
        {
            if (learnerRecords == null || !learnerRecords.Any())
            {
                var message = $"No learners data retrieved from LRS to process gender information. Method: ProcessLearnerGenderAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LrsLearnerGenderResponse { IsSuccess = true, Message = message };
            }

            var profilesToUpdate = new List<TqRegistrationProfile>();
            var registrationProfiles = await _tqRegistrationRepository.GetRegistrationProfilesByIdsAsync(learnerRecords.Select(x => x.ProfileId).ToHashSet());

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
                return new LrsLearnerGenderResponse { IsSuccess = response > 0, LrsCount = learnerRecords.Count(), ModifiedCount = profilesToUpdate.Count(), SavedCount = response };
            }
            else
            {
                return new LrsLearnerGenderResponse { IsSuccess = true, LrsCount = learnerRecords.Count(), ModifiedCount = profilesToUpdate.Count(), SavedCount = 0 };
            }
        }

        private static void ProcessLearningEvents(List<Qualification> qualifications, LrsLearnerRecordDetails learnerRecord)
        {
            if (learnerRecord.IsLearnerVerified == false) return;

            foreach (var learnerEvent in learnerRecord.LearningEventDetails)
            {
                Qualification qualification = qualifications.FirstOrDefault(q => q.IsActive && q.Code.Equals(learnerEvent.QualificationCode, StringComparison.InvariantCultureIgnoreCase));
                IEnumerable<QualificationGrade> grades = qualification?.QualificationType?.QualificationGrades?.Where(g => g.IsActive);

                QualificationGrade qualificationGrade = qualification?.Code == Constants.WJECEduqasEnglishLanguageQualificationCode && learnerEvent.Grade.Length > 1
                    ? grades?.FirstOrDefault(g => g.Grade.Equals(learnerEvent.Grade.Remove(learnerEvent.Grade.Length - 1), StringComparison.InvariantCultureIgnoreCase))
                    : grades?.FirstOrDefault(g => g.Grade.Equals(learnerEvent.Grade, StringComparison.InvariantCultureIgnoreCase));

                if (qualification != null && qualificationGrade != null)
                {
                    learnerEvent.IsQualificationAllowed = true;
                    learnerEvent.IsAchieved = qualificationGrade.IsAllowable;
                    learnerEvent.QualificationGradeId = qualificationGrade.Id;
                    learnerEvent.GradeRank = qualificationGrade.GradeRank;
                    learnerEvent.QualificationId = qualification.Id;
                    learnerEvent.IsEnglishSubject = qualification.TlLookup?.Code.Equals("Eng", StringComparison.InvariantCultureIgnoreCase) ?? false;
                    learnerEvent.IsMathsSubject = qualification.TlLookup?.Code.Equals("Math", StringComparison.InvariantCultureIgnoreCase) ?? false;
                }
            }
        }

        private static TqRegistrationProfile ProcessProfileAndQualificationsAchieved(List<Qualification> qualifications, LrsLearnerRecordDetails learnerRecord, TqRegistrationProfile profile)
        {
            if (learnerRecord.IsLearnerVerified == false && learnerRecord.IsLearnerVerified == profile.IsLearnerVerified)
                return null;

            var isProfileChanged = false;

            // 1. Update Profile with IsLearnerVerified
            if (learnerRecord.IsLearnerVerified != profile.IsLearnerVerified)
            {
                profile.IsLearnerVerified = learnerRecord.IsLearnerVerified;
                profile.ModifiedOn = DateTime.UtcNow;
                profile.ModifiedBy = learnerRecord.PerformedBy;
                isProfileChanged = true;
            }

            // 2. Update Qualifacation achievements
            var learnerLearningEvents = learnerRecord.LearningEventDetails.Where(x => x.IsQualificationAllowed);
            if (learnerLearningEvents.Any() &&
                (IsSubjectStatusUpdateRequired(profile.MathsStatus) || IsSubjectStatusUpdateRequired(profile.EnglishStatus)))
            {
                var isQualificationAchievedChanged = false;

                foreach (var learnerLearningEvent in learnerLearningEvents)
                {
                    var existingQualificationAchieved = profile.QualificationAchieved.FirstOrDefault(q => q.QualificationId == learnerLearningEvent.QualificationId && q.IsAchieved);

                    if (existingQualificationAchieved != null)
                    {
                        var existingQualification = qualifications.FirstOrDefault(q => q.Id == existingQualificationAchieved.QualificationId);
                        var existingQualificationGrade = existingQualification?.QualificationType?.QualificationGrades?.FirstOrDefault(g => g.Id == existingQualificationAchieved.QualificationGradeId);
                        var existingQualificationGradeRank = existingQualificationGrade?.GradeRank ?? 0;

                        // If Lrs send same qualification again but with higher grade then update our Db record. 
                        if (learnerLearningEvent.QualificationGradeId != existingQualificationAchieved.QualificationGradeId && learnerLearningEvent.GradeRank < existingQualificationGradeRank)
                        {
                            existingQualificationAchieved.QualificationGradeId = learnerLearningEvent.QualificationGradeId;
                            existingQualificationAchieved.IsAchieved = learnerLearningEvent.IsAchieved;

                            if (existingQualificationAchieved.Id > 0) // If record existing in database then update modified
                            {
                                existingQualificationAchieved.ModifiedBy = learnerRecord.PerformedBy;
                                existingQualificationAchieved.ModifiedOn = DateTime.UtcNow;
                            }

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
                    if (IsSubjectStatusUpdateRequired(profile.MathsStatus) &&
                        learnerLearningEvents.Any(e => e.IsMathsSubject))
                    {
                        profile.MathsStatus = learnerLearningEvents.Any(e => e.IsMathsSubject && e.IsAchieved) ? SubjectStatus.AchievedByLrs : SubjectStatus.NotAchievedByLrs;
                    }

                    if (IsSubjectStatusUpdateRequired(profile.EnglishStatus) &&
                        learnerLearningEvents.Any(e => e.IsEnglishSubject))
                    {
                        profile.EnglishStatus = learnerLearningEvents.Any(e => e.IsEnglishSubject && e.IsAchieved) ? SubjectStatus.AchievedByLrs : SubjectStatus.NotAchievedByLrs;
                    }

                    profile.ModifiedOn = DateTime.UtcNow;
                    profile.ModifiedBy = learnerRecord.PerformedBy;
                    isProfileChanged = true;
                }
            }

            return isProfileChanged ? profile : null;
        }

        private static bool IsSubjectStatusUpdateRequired(SubjectStatus? subjectStatus)
        {
            return subjectStatus == null || subjectStatus == SubjectStatus.NotSpecified || subjectStatus == SubjectStatus.NotAchievedByLrs;
        }

        private async Task<List<Qualification>> GetAllQualifications()
        {
            return await _qualificationRepository.GetManyAsync(x => x.QualificationType.IsActive && x.IsActive, x => x.QualificationType, x => x.QualificationType.QualificationGrades, x => x.TlLookup).ToListAsync();
        }
    }
}
