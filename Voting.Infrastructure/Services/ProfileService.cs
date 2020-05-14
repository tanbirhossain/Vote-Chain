using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Votin.Model.Exceptions;
using Voting.Infrastructure.API.User;
using Voting.Infrastructure.DTO.Profile;
using Voting.Infrastructure.Model.Common;
using Voting.Infrastructure.Model.Profile;
using Voting.Model.Context;
using Voting.Model.Entities;
using Voting.Model.Exceptions;

namespace Voting.Infrastructure.Services
{
    public class ProfileService
    {
        private readonly BlockchainCommonContext _commonContext;

        public ProfileService(BlockchainCommonContext commonContext)
        {
            _commonContext = commonContext;
        }

        public async Task<WalletDTO> GetNewWallet(CreateWalletModel model)
        {
            Wallet wallet = new Wallet();

            bool isUserRegistered = await _commonContext.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (isUserRegistered)
                throw new ValidationException("The entered mobile number is already registered.");

            WalletDTO result = new WalletDTO
            {
                PublicKey = wallet.PublicKey,
                PrivateKey = wallet.KeyPair.GetPrivateKey()
            };

            _commonContext.Users.Add(new User
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Role = Role.Voter,
                PublicKey = result.PublicKey
            });
            await _commonContext.SaveChangesAsync();
            return result;
        }


        public async Task<WalletQRDTO> GetNewWalletWithQRcode(CreateWalletModel model)
        {
            Wallet wallet = new Wallet();

            bool isUserRegistered = await _commonContext.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (isUserRegistered)
                throw new ValidationException("The entered mobile number is already registered.");

            WalletQRDTO result = new WalletQRDTO
            {
                QRCodeBase64 = QRCodeGenerate(wallet.KeyPair.GetPrivateKey()),
                PublicKey = wallet.PublicKey,
                PrivateKey = wallet.KeyPair.GetPrivateKey()
            };
            _commonContext.Users.Add(new User
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Role = Role.Voter,
                PublicKey = result.PublicKey
            });
            await _commonContext.SaveChangesAsync();
            return result;
        }

        private string QRCodeGenerate(string txtQRCode)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(txtQRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        public async Task<WalletDTO> GetPublicKey(string privateKey)
        {
            Wallet wallet = new Wallet(privateKey);

            WalletDTO result = new WalletDTO
            {
                PublicKey = wallet.PublicKey
            };

            bool isUserRegistered = await _commonContext.Users.AnyAsync(u => u.PublicKey == result.PublicKey);

            if (!isUserRegistered)
                throw new ValidationException("The ID you entered is not valid");

            return result;
        }

        public async Task<WalletUser> GetPublicKeyUser(string privateKey)
        {
            Wallet wallet = new Wallet(privateKey);

            WalletUser result = new WalletUser
            {
                PublicKey = wallet.PublicKey
            };
            var user = await _commonContext.Users.Where(u => u.PublicKey == result.PublicKey).FirstOrDefaultAsync();
            if (user == null)
                throw new ValidationException("The ID you entered is not valid");

            result.Name = user.Name;
            return result;
        }
        public async Task<PagedResult<User>> GetUsersAsync(UserSearch filter)
        {
            PagedResult<User> result = new PagedResult<User>();

            filter.Name = filter.Name ?? "";
            filter.Address = filter.Address ?? "";

            var users = await _commonContext.Users.Where(
                u => u.Name.Contains(filter.Name) &&
                     u.PublicKey.Contains(filter.Address)
            ).OrderByDescending(e => e.Id).ToListAsync();

            result.Items = users;
            result.TotalCount = users.Count;

            return result;
        }

        public async Task<bool> IsAdminAsync(string publicKey)
        {
            var user = await _commonContext.Users.SingleOrDefaultAsync(u => u.PublicKey == publicKey);

            if (user == null)
                throw new NotFoundException("User");

            return user.Role == Role.Admin;
        }

        public async Task<User> GetUsernameAsync(string publicKey)
        {
            var user = await _commonContext.Users.SingleOrDefaultAsync(u => u.PublicKey == publicKey);

            if (user == null)
                throw new NotFoundException("User");

            return new User
            {
                Name = user.Name
            };
        }
    }
}