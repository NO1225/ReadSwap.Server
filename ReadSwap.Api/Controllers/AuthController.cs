using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Interfaces;
using ReadSwap.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReadSwap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IJwtService jwtService)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._jwtService = jwtService;
        }

        /// <summary>
        /// Chech if the passed email is already registered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(nameof(CheckEmailExists))]
        public async Task<ActionResult<ApiRespnse<CheckEmailApiModel.Response>>> CheckEmailExists(CheckEmailApiModel.Request request)
        {
            var response = new ApiRespnse<CheckEmailApiModel.Response>();
            response.Data = new CheckEmailApiModel.Response();

            response.Data.Exists = await (checkEmail(request.Email));
           
            return Ok(response);
        }

        /// <summary>
        /// Create a new Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(nameof(SignUp))]
        public async Task<ActionResult<ApiRespnse<SignUpApiModel.Response>>> SignUp(SignUpApiModel.Request request)
        {
            var response = new ApiRespnse<SignUpApiModel.Response>();

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
        [HttpPost(nameof(SignIn))]
        public async Task<ActionResult<ApiRespnse<SignInApiModel.Response>>> SignIn(SignInApiModel.Request request)
        {
            var response = new ApiRespnse<SignInApiModel.Response>();

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

            response.Data.Token = _jwtService.GenerateAccessToken(new List<Claim>() { 
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,"")
            });

            return Ok(response);
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
