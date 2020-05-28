using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadRegistrationsFileViewModel
    {
        [Required(ErrorMessage = "Select a file to upload")]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}
