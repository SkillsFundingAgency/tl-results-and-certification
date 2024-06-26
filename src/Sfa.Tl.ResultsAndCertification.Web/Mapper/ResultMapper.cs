﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ResultMapper : Profile
    {
        public ResultMapper()
        {
            CreateMap<UploadResultsRequestViewModel, BulkProcessRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}"))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => Guid.NewGuid()))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => FileType.Csv))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => DocumentType.Results))
               .ForMember(d => d.LoginUserType, opts => opts.MapFrom(s => LoginUserType.AwardingOrganisation))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadResultsRequestViewModel, BulkProcessRequest>>());

            CreateMap<BulkResultResponse, UploadResultsResponseViewModel>()
                   .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
                   .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
                   .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<ResultDetails, ResultWithdrawnViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn));

            CreateMap<ManageCoreResultViewModel, AddResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.LookupId, opts => opts.MapFrom(s => s.LookupId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ManageCoreResultViewModel, AddResultRequest>>());

            CreateMap<ManageSpecialismResultViewModel, AddResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.LookupId, opts => opts.MapFrom(s => s.LookupId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Specialism))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ManageSpecialismResultViewModel, AddResultRequest>>());

            CreateMap<LearnerRecord, ManageCoreResultViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).SeriesName?.ToLowerInvariant()))
                .ForMember(d => d.ResultEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ResultEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Result?.Id))
                .ForMember(d => d.CoreGradeCode, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Result?.GradeCode))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Result?.PrsStatus))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<ResultDetails, ManageCoreResultViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayLarId})"))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.ResultEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
                .ForMember(d => d.SelectedGradeCode, opts => opts.MapFrom(s => s.PathwayResultId.HasValue ? s.PathwayResultCode : string.Empty))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<ManageCoreResultViewModel, ChangeResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
                .ForMember(d => d.LookupId, opts => opts.MapFrom(s => s.LookupId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ManageCoreResultViewModel, ChangeResultRequest>>());

            CreateMap<LearnerRecord, ResultDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreComponentDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.CoreComponentExams, opts => opts.MapFrom(s => s.Pathway.PathwayAssessments))
                .ForMember(d => d.SpecialismComponents, opts => opts.MapFrom(s => s.Pathway.Specialisms));

            CreateMap<Specialism, SpecialismComponentViewModel>()
                .ForMember(d => d.LarId, opts => opts.MapFrom(s => s.LarId))
                .ForMember(d => d.SpecialismComponentDisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"))
                .ForMember(d => d.SpecialismComponentExams, opts => opts.MapFrom(s => s.Assessments))
                .ForMember(d => d.TlSpecialismCombinations, opts => opts.MapFrom(s => s.TlSpecialismCombinations));

            CreateMap<Assessment, ComponentExamViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.ProfileId]))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.ResultEndDate, opts => opts.MapFrom(s => s.ResultEndDate))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom(s => s.RommEndDate))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Result != null ? s.Result.Grade : null))
                .ForMember(d => d.GradeCode, opts => opts.MapFrom(s => s.Result != null ? s.Result.GradeCode : null))
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedOn.ToDobFormat() : null))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedBy : null))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => s.Result != null ? s.Result.PrsStatus : null));

            CreateMap<LearnerRecord, ManageSpecialismResultViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => ((Specialism)context.Items["specialism"]).Name))
                .ForMember(d => d.SpecialismDisplayName, opts => opts.MapFrom((src, dest, destMember, context) => $"{((Specialism)context.Items["specialism"]).Name} ({((Specialism)context.Items["specialism"]).LarId})"))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).SeriesName?.ToLowerInvariant()))
                .ForMember(d => d.ResultEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ResultEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Result?.Id))
                .ForMember(d => d.GradeCode, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Result?.GradeCode))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Result?.PrsStatus))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<ManageSpecialismResultViewModel, ChangeResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
                .ForMember(d => d.LookupId, opts => opts.MapFrom(s => s.LookupId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Specialism))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ManageSpecialismResultViewModel, ChangeResultRequest>>());
        }
    }
}