using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations.DateValidatorAttributeTests
{
    public abstract class DateValidatorAttributeBaseTests: BaseTest<DateValidatorAttribute>
    {
        protected RequiredDateViewModel model;
        protected ValidationContext validationContext;
        protected List<ValidationResult> validationResults;
        protected bool overAllResult;

        public override void Setup()
        {
            validationResults = new List<ValidationResult>();
        }

        public class RequiredDateViewModel
        {
            [DateValidator(Property = nameof(RequiredDate), ErrorResourceType = typeof(ErrorResource.ReviewChangeStartYear), ErrorResourceName = "Validation_Date_When_Change_Requested_Blank_Text")]
            public string RequiredDate => $"{Day}/{Month}/{Year}";
            public string Day { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }
    }
}
