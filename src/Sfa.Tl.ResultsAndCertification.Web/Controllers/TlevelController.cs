using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class TlevelController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAwardingOrganisationLoader _awardingOrganisationLoader;

        public TlevelController(IAwardingOrganisationLoader awardingOrganisationLoader, IMapper mapper)
        {
            _awardingOrganisationLoader = awardingOrganisationLoader;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // TODO: How to get the loggedin AO userId?
            var data = await _awardingOrganisationLoader.GetTlevelsByAwardingOrganisationAsync(1);
            
            var viewModel = _mapper.Map<IEnumerable<YourTlevelsViewModel>>(data);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            return await Task.Run(() => View());
        }
    }
}