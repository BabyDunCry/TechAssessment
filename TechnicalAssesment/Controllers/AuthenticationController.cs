using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechnicalAssesment.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public string Index()
        {
            var authCode = "abcdefg01234567890";
           return authCode;
        }
    }
}