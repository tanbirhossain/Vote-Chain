using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;
using Voting.API.Controllers.Util;
using Voting.Infrastructure.API.Vote;
using Voting.Infrastructure.Services;

namespace Voting.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VotingController : BaseController
    {
        private readonly VotingService _votingService;

        public VotingController(VotingService votingService, IHttpContextAccessor httpContext) : base(httpContext)
        {
            _votingService = votingService;
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] List<Vote> votes)
        {
            await _votingService.Vote(votes, PrivateKey);
            return Ok();
        }

        [HttpGet]
        public IActionResult TestKeys()
        {
            List<string> result = new List<string>();

            for (int i = 0; i < 20; i++)
            {
                var key = EthECKey.GenerateKey();
                result.Add(key.GetPublicAddress());
            }

            return Ok(result);
        }
    }

}