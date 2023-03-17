using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IaDbContext _context;

        public ChartsController(IaDbContext context)
        {
            _context = context;
        }

        [HttpGet("OperationsNumber")]
        public JsonResult OperationsNumber()
        {
            List<object> depOperations = new List<object>();
            depOperations.Add(new [] {"Департамент", "Кількість операцій"});
            foreach (var department in _context.Departments.ToList())
            {
                depOperations.Add(new object[]
                {
                    department.Name,
                    _context.Operations.Count(operation => operation.DepartmentId == department.Id)
                });
            }
            
            return new JsonResult(depOperations);
        }
        
        [HttpGet("OperationWorkers")]
        public JsonResult OperationWorkers()
        {
            List<object> workersNum = new List<object>();
            workersNum.Add(new [] {"Операція", "Кількість учасників"});
            foreach (var operation in _context.Operations.ToList())
            {
                workersNum.Add(new object[]
                {
                    operation.Name,
                    _context.WorkersToOps.Count(op => op.OperationId == operation.Id)
                });
            }
            
            return new JsonResult(workersNum);
        }
    }
}
