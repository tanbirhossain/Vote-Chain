using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chain.Admin.API.Controller.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Infrastructure.API.Vote;
using Voting.Infrastructure.DTO.Election;
using Voting.Infrastructure.Model.Common;
using Voting.Infrastructure.Model.Election;
using Voting.Infrastructure.Services;

namespace Chain.Admin.Areas.Auth.Controllers
{
    [Route("api/auth/[controller]/[action]")]
    [ApiController]
    public class ElectionController : BaseController
    {
        private readonly ElectionService _electionService;
        private readonly VotingService _votingService;
        public ElectionController( VotingService votingService, ElectionService electionService, IHttpContextAccessor contextAccessor)
          : base(contextAccessor)
        {
            _electionService = electionService;
            _votingService = votingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetElectionList([FromQuery] ElectionSearch model)
        {
            PagedResult<ElectionDTO> elections = await _electionService.GetElectionsListAsync(model);

            return Ok(elections);
        }
       
        [HttpGet]
        public async Task<IActionResult> GetElectionDetails(int electionId)
        {
            ElectionDTO election = await _electionService.GetElectionDetailsByIdAsync(electionId);

            return Ok(election);
        }

        [HttpPost]
        public async Task<IActionResult> UserVote(Vote vote)
        {
            await _votingService.UserVote(vote, PrivateKey);
            return Ok();
        }

    }
}