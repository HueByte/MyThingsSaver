using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.lib;
using MTS.Core.Services.LegalNotice;

namespace MTS.App.Controllers
{
    public class LegalNoticeController : BaseApiController
    {
        private readonly LegalNoticeService _privacyPolicyService;
        public LegalNoticeController(LegalNoticeService privacyPolicyService)
        {
            _privacyPolicyService = privacyPolicyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponse<string>), 200)]
        public Task<IActionResult> Get()
        {
            return Task.FromResult(ApiResponse.Property(_privacyPolicyService.LEGAL_NOTICE));
        }
    }
}