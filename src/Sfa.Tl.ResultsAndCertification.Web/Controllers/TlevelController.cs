using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class TlevelController : Controller
    {
        private readonly IAwardingOrganisationLoader _awardingOrganisationLoader;
        

        public TlevelController(IAwardingOrganisationLoader awardingOrganisationLoader)
        {
            _awardingOrganisationLoader = awardingOrganisationLoader;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _awardingOrganisationLoader.GetTlevelsByAwardingOrganisationAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = new YourTLevelDetailsViewModel
            {
                PageTitle = "T Level details",
                RouteName = "Construction",
                PathwayName = "Construction: Design, Surveying and Planning",
                Specialisms = new List<string>
                {
                    "Surveying and design for construction and the built environment",
                    "Civil engineering",
                    "Building services design",
                    "Hazardous materials analysis and surveying"
                }
            };

            return await Task.Run(() => View(viewModel));
        }
    }
}