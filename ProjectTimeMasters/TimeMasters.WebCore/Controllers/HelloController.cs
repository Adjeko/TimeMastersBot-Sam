using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeMasters.WebCore.Controllers
{
    [Route("api/[controller]")]
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
