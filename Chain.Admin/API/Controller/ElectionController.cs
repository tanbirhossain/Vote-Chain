using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chain.Admin.API.Controller.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Infrastructure.API.Election;
using Voting.Infrastructure.DTO.Election;
using Voting.Infrastructure.Model.Common;
using Voting.Infrastructure.Model.Election;
using Voting.Infrastructure.Services;

namespace Chain.Admin.API.Controller
{
    [Route("api/[controller]/[action]")]
    public class ElectionController : ControllerBase// BaseController
    {
        private readonly ElectionService _electionService;

        public ElectionController(ElectionService electionService, IHttpContextAccessor contextAccessor) 
          //  : base(contextAccessor)
        {
            _electionService = electionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetElections([FromQuery] ElectionSearch model)
        {
            PagedResult<ElectionDTO> elections = await _electionService.GetElectionsAsync(model);
           
            return Ok(elections);
        }
        [HttpGet]
        public async Task<IActionResult> GetElection(int electionId)
        {
            ElectionDTO election = await _electionService.GetElectionAsync(electionId);

            return Ok(election);
        }
        [HttpGet]
        public async Task<IActionResult> GetElectionVotes()
        {
            List<ElectionVotes> result = await _electionService.GetELectionVotesAsync();

            return Ok(result);
        }


        //Here is election

        [HttpPost]
        public async Task<IActionResult> CreateElection(CreateElection election)
        {
            await _electionService.CreateElectionAsync(election);
            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateElection([FromBody] UpdateElection election)
        {
            await _electionService.UpdateElectionAsync(election);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> CloseElection([FromBody]UpdateElection election)
        {
            await _electionService.CloseElectionAsync(election.Id);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> OpenElection([FromBody]UpdateElection election)
        {
            await _electionService.OpenElectionAsync(election.Id);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveElection(int electionId)
        {
            await _electionService.RemoveElectionAsync(electionId);

            return Ok();
        }
    }
}