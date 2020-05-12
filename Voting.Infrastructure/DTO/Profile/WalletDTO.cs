﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Infrastructure.DTO.Profile
{
    public class WalletDTO
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }

    public class WalletUser
    {
        public string Name { get; set; }
        public string PublicKey { get; set; }
    }
}
