using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadSwap.Api.Helpers;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Interfaces;
using ReadSwap.Core.Models;

namespace ReadSwap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public TokenController(ITokenService tokenService,UserManager<AppUser> userManager)
        {
            this._tokenService = tokenService;
            this._userManager = userManager;
        }
        
        /// <summary>
        /// Issue a new access token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(Refresh))]
        public async Task<ActionResult<ApiResponse<TokenApiModel.Response>>> Refresh(TokenApiModel.Request model)
        {
            var princibles = _tokenService.GetClaimsFromExpiredToken(model.AccessToken);

            var user = await _userManager.GetUserAsync(princibles);

            if(user == null)
            {
                return BadRequest("Invaled Access Token");
            }

            if(model.RefreshToken != user.RefreshToken)
            {
                return BadRequest("Invaled Refresh Token");
            }

            var response = new ApiResponse<TokenApiModel.Response>();

            response.Data = new TokenApiModel.Response();
            response.Data.RefreshToken = _tokenService.GenerateRefreshToken(); ;

            user.RefreshToken = response.Data.RefreshToken;          

            response.Data.AccessToken = _tokenService.GenerateAccessToken(await user.GetClaimsAsync(_userManager));

            await _userManager.UpdateAsync(user);

            return response;
        }

        /// <summary>
        /// Delete the current refresh token of the current user
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(Revoke))]
        [Authorize]
        public async Task<ApiResponse> Revoke()
        {
            var user = await _userManager.GetUserAsync(User);
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return new ApiResponse();
        }
    }
}
