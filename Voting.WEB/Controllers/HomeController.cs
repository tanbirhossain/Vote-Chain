using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Voting.Infrastructure.PeerToPeer;
using Voting.WEB.Models;

namespace Voting.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly P2PNetwork _p2PNetwork;

        public HomeController(ILogger<HomeController> logger, P2PNetwork p2PNetwork)
        {
            _logger = logger;
            _p2PNetwork = p2PNetwork;
        }

        public IActionResult Index()
        {

            List<string> list = new List<string>();
            var result = _p2PNetwork.GetSockets;
            foreach (var item in result)
            {
                list.Add($"{((IPEndPoint)(item.RemoteEndPoint)).Address.ToString()}:{((IPEndPoint)(item.RemoteEndPoint)).Port.ToString()}");
            }
            ViewBag.ConnectedList = list;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
