using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Infrastructure.Services;
using Voting.Model.Entities;

namespace Chain.Admin.API.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlockChainController : ControllerBase
    {
        private readonly MinerService _minerService;
        public BlockChainController(MinerService minerService)
        {
            _minerService = minerService;
        }

        [HttpGet]
        public async Task<IActionResult> MineTransaction()
        {
            Block block = await _minerService.Mine();

            return Ok(block);
        }
    }

}