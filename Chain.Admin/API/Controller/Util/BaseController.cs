using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Model.Exceptions;

namespace Chain.Admin.API.Controller.Util
{

    public class BaseController : ControllerBase
    {
        protected bool ApplyAuthorization { get; set; } = true;
        protected string PublicKey { get; set; }
        protected string PrivateKey { get; set; }

        public BaseController(IHttpContextAccessor httpContext)
        {
            InitWallet(httpContext);
        }

        private void InitWallet(IHttpContextAccessor httpContext)
        {
            PrivateKey = httpContext.HttpContext.Request.Headers["PrivateKey"];
            PublicKey = httpContext.HttpContext.Request.Headers["PublicKey"];

            if (ApplyAuthorization && (string.IsNullOrEmpty(PrivateKey) || string.IsNullOrEmpty(PublicKey)))
                throw new UnauthorizedException("Lack of access, log in again");
        }
    }

}