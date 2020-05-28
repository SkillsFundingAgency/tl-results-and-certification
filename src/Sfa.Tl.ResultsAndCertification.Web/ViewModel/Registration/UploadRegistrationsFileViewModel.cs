using Microsoft.AspNetCore.Http;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadRegistrationsFileViewModel
    {
        [DataType(DataType.Upload)]
        [Required(ErrorMessageResourceType = typeof(ErrorResource.Upload), ErrorMessageResourceName = "Select_File_To_Upload_Required_Validation_Message")]
        [AllowedExtensions(".csv", ErrorMessageResourceType = typeof(ErrorResource.Upload), ErrorMessageResourceName = "Must_Be_Csv_Validation_Message")]
        public IFormFile File { get; set; }
    }
}
