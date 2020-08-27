using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Models;
using ReadSwap.Data;

namespace ReadSwap.Api.Controllers
{
    /// <summary>
    /// To manage the user profile
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataAccess _dataAccess;

        public ProfileController(UserManager<AppUser> userManager, DataAccess dataAccess)
        {
            this._userManager = userManager;
            this._dataAccess = dataAccess;
        }

        /// <summary>
        /// Creating the user Profile
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateProfile(CreateProfileApiModel.Request requestModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = new Profile()
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Address = requestModel.Address,
                User = user
            };

            await _dataAccess.Profiles.AddAsync(profile);
            await _dataAccess.SaveChangesAsync();

            return Ok(new ApiResponse());
        }
    }
}
