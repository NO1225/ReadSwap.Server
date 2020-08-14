using System;
using System.Collections.Generic;
using System.Text;

namespace ReadSwap.Core.Models
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public int Lifetime { get; set; }
    }
}
