using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chain.Admin.API.Controller.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Infrastructure.DTO.Election;
using Voting.Infrastructure.Services;

namespace Chain.Admin.API.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CandidateController : BaseController
    {
        private readonly ElectionService _electionService;


        public CandidateController(ElectionService electionService, IHttpContextAccessor contextAccessor) : base(
            contextAccessor)
        {
            _electionService = electionService;
        }

        /// <summary>
        /// Elections which current user has not voted yet !!
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUnvotedElections()
        {
            List<ElectionDTO> elections = await _electionService.GetUnvotedElections(PublicKey);
            return Ok(elections);
        }

        [HttpGet]
        public async Task<IActionResult> GetParticipatedElections()
        {
            List<ParticipatedElection> participatedElections =
                await _electionService.GetParticipatedElectionsAsync(PublicKey);
            return Ok(participatedElections);
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidatedElections()
        {
            List<CandidatedElection> candidatedElections =
                await _electionService.CandidatedElectionAsync(PublicKey);

            return Ok(candidatedElections);
        }



    }
}