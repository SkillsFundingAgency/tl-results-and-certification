using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registrations
{
    public class UploadRegistrationsFileViewModel
    {
        [Required(ErrorMessage = "Select a file to upload")]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}
