using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    [Authorize(Roles="admin, chairman, commander, hr")]
    public class HumanResourcesController : Controller
    {
        // GET: Archive
        public IActionResult Index()
        {
            return View();
        }
    }
}