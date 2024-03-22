using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink
{
    public class ChangeRecord : ViewComponent
    {
        public IViewComponentResult Invoke(ChangeRecordModel model)
        {
            return View("~/ViewComponents/ChangeRecordLink/Default.cshtml", model);
        }
    }
}