using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Oauth.Controllers
{
    public class ProtectedController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
