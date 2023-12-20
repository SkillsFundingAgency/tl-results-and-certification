using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminDashboardMapper : Profile
    {
        public AdminDashboardMapper()
        {
            CreateMap<AdminLearnerRecord, AdminLearnerRecord>()
                .ForMember(p => p.DisplayAcademicYear, opts => opts.ConvertUsing(new AcademicYearConverter(), p => p.AcademicYear));
        }
    }
}
