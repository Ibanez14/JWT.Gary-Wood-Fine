﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.GrayWoodFine.webAPI.Models
{
    public class TokenRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
