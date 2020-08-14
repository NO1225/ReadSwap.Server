using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ReadSwap.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(List<Claim> claims);
    }
}
