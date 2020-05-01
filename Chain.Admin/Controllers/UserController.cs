using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chain.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }


        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }
       
    }
}