﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class UploadRommsRequestViewModel
    {
        public long AoUkprn { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessageResourceType = typeof(ErrorResource.UploadRomms), ErrorMessageResourceName = "Select_File_To_Upload_Required_Validation_Message")]
        [FileValidation(AllowedExtensions = ".csv", MaxFileNameLength = 150, MaxFileSizeInMb = Constants.MaxFileSizeInMb, MaxRecordCount = 10000, ErrorResourceType = typeof(ErrorResource.UploadRomms))]
        public IFormFile File { get; set; }

        public int? RequestErrorTypeId { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.ResultReviewsAndAppeals, RouteName = RouteConstants.ResultReviewsAndAppeals },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Upload_Romms_File }
                    }
                };
            }
        }

        public void SetAnyModelErrors(ModelStateDictionary modelState)
        {
            if (modelState != null && IsValidErrorType)
            {
                var error = ModelStateHelper.GetUploadErrorMessage((RequestErrorType)RequestErrorTypeId, typeof(ErrorResource.UploadRomms));

                if (error != null)
                {
                    ModelStateHelper.AddModelStateError(modelState, error.Item1, error.Item2);
                }
            }
        }

        private bool IsValidErrorType => EnumExtensions.IsValidValue<RequestErrorType>(RequestErrorTypeId);
    }
}
