using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Votin.Model.Exceptions;
using Voting.Infrastructure.API.Vote;
using Voting.Infrastructure.PeerToPeer;
using Voting.Model.Context;
using Voting.Model.Entities;
using Voting.Model.Exceptions;

namespace Voting.Infrastructure.Services
{
    public class VotingService
    {
        private readonly BlockchainContext _dbContext;
        private readonly BlockchainCommonContext _commonContext;
        private readonly WalletService _walletService;
        private readonly MinerService _minerService;
        private readonly P2PNetwork _p2PNetwork;

        public VotingService(BlockchainContext dbContext, WalletService walletService, MinerService minerService,
            P2PNetwork p2PNetwork, BlockchainCommonContext commonContext)
        {
            _dbContext = dbContext;
            _commonContext = commonContext;
            _walletService = walletService;
            _minerService = minerService;
            _p2PNetwork = p2PNetwork;
        }

        public async Task Vote(List<Vote> votes, string privateKey)
        {
            Wallet wallet = new Wallet(privateKey);

            foreach (var vote in votes)
            {
                var election =
                    await _commonContext.Elections.SingleOrDefaultAsync(e => e.Address == vote.ElectionAddress);

                if (election == null)
                    throw new NotFoundException("election");

                if (election.Status == ElectionStatus.Closed)
                    throw new ValidationException($"election {election.Address} It is finished");
            }

            foreach (var vote in votes)
            {
                Transaction transaction =
                    await _walletService.CreateTransaction(wallet, vote.ElectionAddress, vote.Candidate);

                _p2PNetwork.BroadcastTransaction(transaction);
            }
        }

        public async Task UserVote(Vote vote, string privateKey)
        {
            Wallet wallet = new Wallet(privateKey);


            var election =
                await _commonContext.Elections.SingleOrDefaultAsync(e => e.Address == vote.ElectionAddress);

            if (election == null)
                throw new NotFoundException("election");

            if (election.Status == ElectionStatus.Closed)
                throw new ValidationException($"election {election.Address} It is finished");



            Transaction transaction =
                await _walletService.CreateTransaction(wallet, vote.ElectionAddress, vote.Candidate);

            _p2PNetwork.BroadcastTransaction(transaction);

        }



    }
}