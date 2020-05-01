using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chain.Admin.Controllers
{
    
    public class ElectionViewController : Controller
    {
        // GET: Profile
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: Profile
        public async Task<ActionResult> Create()
        {
            return View();
        }

    }
}