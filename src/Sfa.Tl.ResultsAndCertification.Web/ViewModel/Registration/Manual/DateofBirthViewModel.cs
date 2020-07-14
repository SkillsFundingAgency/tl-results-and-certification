using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class DateofBirthViewModel
    {
        public string Day { get; set; }

        public string Month { get; set; }
        
        public string Year { get; set; }

        //public DateTime DateofBirth { get { return string.Concat(Day, Month, Year).ParseStringToDateTime(); } }
        public DateTime DateofBirth { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AddRegistrationLearnersName
                };
            }
        }
    }
}
