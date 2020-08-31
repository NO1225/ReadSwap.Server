using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReadSwap.Api.Helpers;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Entities;
using ReadSwap.Core.Interfaces;
using ReadSwap.Core.Models;
using ReadSwap.Core.Routes;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReadSwap.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._tokenService = tokenService;
        }

        /// <summary>
        /// Chech if the passed email is already registered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.CheckEmailExists)]
        public async Task<ActionResult<ApiResponse<CheckEmailApiModel.Response>>> CheckEmailExists(CheckEmailApiModel.Request request)
        {
            var response = new ApiResponse<CheckEmailApiModel.Response>();
            response.Data = new CheckEmailApiModel.Response();

            response.Data.Exists = await (checkEmail(request.Email));
           
            return Ok(response);
        }

        /// <summary>
        /// Create a new Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.SignUp)]
        public async Task<ActionResult<ApiResponse<SignUpApiModel.Response>>> SignUp(SignUpApiModel.Request request)
        {
            var response = new ApiResponse<SignUpApiModel.Response>();

            if(await checkEmail(request.Email))
            {
                response.AddError(1);
                return response;
            }

            var appUser = new AppUser()
            {
                Email = request.Email,
                UserName = request.Email

            };

            var result = await _userManager.CreateAsync(appUser, request.Passward);

            if(result.Succeeded == false)
            {
                // TODO:
                return NotFound();
            }

            response.Data = new SignUpApiModel.Response() { 
                Email=request.Email
            };

            return Ok(response);
        }

        /// <summary>
        /// Signing in to the account and aqcuaring the token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.SignIn)]
        public async Task<ActionResult<ApiResponse<SignInApiModel.Response>>> SignIn(SignInApiModel.Request request)
        {
            var response = new ApiResponse<SignInApiModel.Response>();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user==null)
            {
                response.AddError(2);
                return (response);
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Passward, false, false);

            if (result.Succeeded == false)
            {
                response.AddError(3);
                return (response);
            }

            response.Data = new SignInApiModel.Response();


            response.Data.AccessToken = _tokenService.GenerateAccessToken(await user.GetClaimsAsync(_userManager));
            response.Data.RefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = response.Data.RefreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(response);
        }

        /// <summary>
        /// Changing the password of the current user
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.ChangePassword)]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> ChangePassword(ChangePasswordApiModel.Request requestModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var responseModel = new ApiResponse();

            var correctPassword = await _userManager.CheckPasswordAsync(user, requestModel.OldPassword);

            if(correctPassword == false)
            {
                responseModel.AddError(6);
                return Ok(responseModel);
            }

            var result = await _userManager.ChangePasswordAsync(user, requestModel.OldPassword,requestModel.NewPassward);

            if(result.Succeeded == false)
            {
                // TODO: Get the errors out
                responseModel.AddError(7);
            }
            else
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return Ok(responseModel);
        }


        #region Private Helper

        /// <summary>
        /// Check if the email already registered
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<bool> checkEmail(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        } 

        #endregion
    }
}
