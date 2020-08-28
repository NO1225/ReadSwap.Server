using Microsoft.AspNetCore.Identity;
using ReadSwap.Core.Entities;
using ReadSwap.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadSwap.Api.Helpers
{
    public static class UserHelper
    {
        /// <summary>
        /// Return the claims of the passed user to be used in token generation
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Claim>> GetClaimsAsync(this AppUser user,UserManager<AppUser> userManager)
        {
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
