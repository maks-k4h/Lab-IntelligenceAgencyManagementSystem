using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    public class HumanResourcesController : Controller
    {
        // GET: Archive
        public IActionResult Index()
        {
            return View();
        }
    }
}