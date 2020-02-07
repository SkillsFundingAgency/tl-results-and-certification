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
        private readonly IMapper _mapper;

        public TlevelController(IAwardingOrganisationLoader awardingOrganisationLoader, IMapper mapper)
        {
            _awardingOrganisationLoader = awardingOrganisationLoader;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // TODO: following statement to be updated.
            var id = !string.IsNullOrEmpty(User.GetUkPrn()) ? int.Parse(User.GetUkPrn()) : 10009696;

            var tLevels = await _awardingOrganisationLoader.GetTlevelsByAwardingOrganisationAsync(id);
            var viewModel = _mapper.Map<IEnumerable<YourTlevelsViewModel>>(tLevels);
            
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            return await Task.Run(() => View());
        }
    }
}