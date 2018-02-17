using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class TestRouteController : Controller
    {
        private static string[] _primaryRoutes = new[]
        {
            "Vader", "Yoda", "Windu", "Rey", "Skywalker", "Kenobi", "Grievous", "Leia", "Solo"
        };

        public TestRouteController() { }

        public TestRouteController(string[] routes)
        {
            _primaryRoutes = routes;
        }

        [HttpGet("[action]")]
        public IEnumerable<string> GetRoutes()
        {
            return _primaryRoutes;
        }

        // public class Character
        // {
        //     public string Name { get; set; }
        //     public int Points { get; set; }
        //     public SByte Data { get; set; }
        // }
    }
}
