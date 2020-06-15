using Microsoft.AspNetCore.Http;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadRegistrationsRequestViewModel
    {
        public long AoUkprn { get; set; }
         
        [DataType(DataType.Upload)]
        [Required(ErrorMessageResourceType = typeof(ErrorResource.Upload), ErrorMessageResourceName = "Select_File_To_Upload_Required_Validation_Message")]
        [FileValidation(AllowedExtensions = ".csv", MaxFileNameLength = 256, MaxFileSizeInMb = 5, MaxRecordCount = 10000)]
        public IFormFile File { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Registration_Dashboard, RouteName = RouteConstants.RegistrationDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Upload_Registrations_File }
                    }
                };
            }
        }
    }
}
