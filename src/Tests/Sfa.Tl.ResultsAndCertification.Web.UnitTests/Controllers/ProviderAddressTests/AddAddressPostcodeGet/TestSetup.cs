﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressPostcodeGet
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public bool ShowPostcode { get; set; } = true;
        public bool IsFromAddressMissing { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddAddressPostcodeAsync(ShowPostcode, IsFromAddressMissing);
        }
    }
}