﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Infrastructure.API.Election;
using Voting.Infrastructure.API.User;
using Voting.Infrastructure.Model.Profile;
using Voting.Infrastructure.Services;

namespace Chain.Admin.API.Controller
{
    [Route("api/[controller]/[action]")]
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

        [HttpPost]
        public async Task<IActionResult> GetNewWallet(CreateWalletModel model)
        {
            var wallet = await _profileService.GetNewWallet(model);
            return Ok(wallet);
        }

        [HttpPost]
        public async Task<IActionResult> GetNewWalletWithQR(CreateWalletModel model)
        {
            var wallet = await _profileService.GetNewWalletWithQRcode(model);
            return Ok(wallet);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKey(string privateKey)
        {
            var result = await _profileService.GetPublicKey(privateKey);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKeyUser(string privateKey)
        {
            var result = await _profileService.GetPublicKeyUser(privateKey);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserSearch filter)
        {
            var result = await _profileService.GetUsersAsync(filter);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> IsAdmin(string publicKey)
        {
            var isAdmin = await _profileService.IsAdminAsync(publicKey);
            return Ok(isAdmin);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsername(string publicKey)
        {
            var user = await _profileService.GetUsernameAsync(publicKey);
            return Ok(user);
        }

    }


}