using ACMWebApi.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMWebApi
{
    public class BaseController : ControllerBase
    {
        public string data = "";
        public User loginUser;
        public BaseController()
        {
        }
    }
}
