﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireReviewsAndAppealsEditorAccess)]
    public class PostResultsServiceController : Controller
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.PrsCacheKey); } }

        public PostResultsServiceController(ICacheService cacheService, ILogger logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("reviews-and-appeals", Name = RouteConstants.StartReviewsAndAppeals)]
        public IActionResult StartReviewsAndAppeals()
        {
            return View(new StartReviewsAndAppealsViewModel());
        }

        [HttpGet]
        [Route("reviews-and-appeals-search-learner", Name = RouteConstants.SearchPostResultsService)]
        public async Task<IActionResult> SearchPostResultsServiceAsync()
        {
            await Task.CompletedTask;
            return View(new SearchPostResultsServiceViewModel());
        }

        [HttpPost]
        [Route("reviews-and-appeals-search-learner", Name = RouteConstants.SubmitSearchPostResultsService)]
        public async Task<IActionResult> SearchPostResultsServiceAsync(SearchPostResultsServiceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await Task.CompletedTask;
            if (model.SearchUln == "9999999999")
            {
                await _cacheService.SetAsync(CacheKey, new PostResultServiceUlnNotFoundViewModel { Uln = model.SearchUln }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PostResultServiceUlnNotFound);
            }

            return View(new SearchPostResultsServiceViewModel());
        }

        [HttpGet]
        [Route("no-learner-found", Name = RouteConstants.PostResultServiceUlnNotFound)]
        public async Task<IActionResult> PostResultServiceUlnNotFoundAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<PostResultServiceUlnNotFoundViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PostResultServiceUlnNotFoundViewModel from redis cache in request Prs Uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }
    }
}
