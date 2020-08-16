using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ReadSwap.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetClaimsFromExpiredToken(string token);
    }
}
