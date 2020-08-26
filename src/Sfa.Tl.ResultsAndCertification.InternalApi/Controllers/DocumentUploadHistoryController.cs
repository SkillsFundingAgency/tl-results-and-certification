using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentUploadHistoryController : ControllerBase, IDocumentUploadHistoryController
    {
        private readonly IDocumentUploadHistoryService _documentUploadHistoryService;
        private readonly ILogger<DocumentUploadHistory> _logger;
        public DocumentUploadHistoryController(IDocumentUploadHistoryService documentUploadHistoryService, ILogger<DocumentUploadHistory> logger)
        {
            _documentUploadHistoryService = documentUploadHistoryService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetDocumentUploadHistoryDetails/{aoUkprn}/{blobUniqueReference}")]
        public async Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetailsAsync(long aoUkprn, Guid blobUniqueReference)
        {
            return await _documentUploadHistoryService.GetDocumentUploadHistoryDetails(aoUkprn, blobUniqueReference);
        }
    }
}
