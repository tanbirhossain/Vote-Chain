using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Infrastructure.API.User;
using Voting.Infrastructure.Model.Profile;
using Voting.Infrastructure.Services;

namespace Chain.Admin.Areas.Wallet.Controllers
{
    [Route("api/wallet/[controller]/[action]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly WalletService _walletService;
        private readonly ElectionService _electionService;
        public ProfileController(
            ProfileService profileService,
            WalletService walletService,
            ElectionService electionService
        )
        {
            _walletService = walletService;
            _profileService = profileService;
            _electionService = electionService;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetPublicKeyUser(string privateKey)
        {
            var result = await _profileService.GetPublicKeyUser(privateKey);
            return Ok(result);
        }
    }

}