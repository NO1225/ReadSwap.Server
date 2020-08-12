using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Models;
using System.Threading.Tasks;

namespace ReadSwap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
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
