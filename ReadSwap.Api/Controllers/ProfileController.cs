using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Entities;
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
        [HttpPost(nameof(CreateMyProfile))]
        public async Task<ActionResult<ApiResponse>> CreateMyProfile(CreateProfileApiModel.Request requestModel)
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

        /// <summary>
        /// Get the profile of the current logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetMyProfile))]
        public async Task<ActionResult<ApiResponse<GetProfileApiModel.Response>>> GetMyProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var responseModel = new ApiResponse<GetProfileApiModel.Response>();

            var profile = await _dataAccess.Profiles.FirstOrDefaultAsync(profile => profile.UserId == user.Id);

            if(profile != null)
            {
                responseModel.Data = new GetProfileApiModel.Response()
                {
                    Id = profile.Id,
                    Address = profile.Address,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    // TODO: Add the proper rating
                    Rating = 0
                };
            }

            return Ok(responseModel);
        }


        /// <summary>
        /// Edit the profile of the currently logged in user
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(EditMyProfile))]
        public async Task<ActionResult<ApiResponse<EditProfileApiModel.Response>>> EditMyProfile(EditProfileApiModel.Request requestModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var responseModel = new ApiResponse<GetProfileApiModel.Response>();

            var profile = await _dataAccess.Profiles.FirstOrDefaultAsync(profile => profile.UserId == user.Id);

            if (profile != null)
            {
                bool editted = false;
                if(!string.IsNullOrEmpty(requestModel.FirstName) && requestModel.FirstName != profile.FirstName )
                {
                    editted = true;
                    profile.FirstName = requestModel.FirstName;
                }
                if (!string.IsNullOrEmpty(requestModel.LastName) && requestModel.LastName != profile.LastName)
                {
                    editted = true;
                    profile.LastName = requestModel.LastName;
                }
                if (!string.IsNullOrEmpty(requestModel.Address) && requestModel.Address != profile.Address)
                {
                    editted = true;
                    profile.Address = requestModel.Address;
                }

                if(editted == true)
                {
                    _dataAccess.Profiles.Update(profile);
                    await _dataAccess.SaveChangesAsync();
                }

                responseModel.Data = new GetProfileApiModel.Response()
                {
                    Id = profile.Id,
                    Address = profile.Address,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    // TODO: Add the proper rating
                    Rating = 0
                };
            }
            else
            {
                responseModel.AddError(4);
            }

            return Ok(responseModel);
        }

        /// <summary>
        /// Get the profile which have the passed id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProfile/{id}")]
        public async Task<ActionResult<ApiResponse<GetProfileApiModel.Response>>> GetProfile(int id)
        {
            var responseModel = new ApiResponse<GetProfileApiModel.Response>();

            var profile = await _dataAccess.Profiles.FirstOrDefaultAsync(profile => profile.Id == id);

            if (profile != null)
            {
                responseModel.Data = new GetProfileApiModel.Response()
                {
                    Id = profile.Id,
                    Address = profile.Address,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    // TODO: Add the proper rating
                    Rating = 0
                };
            }

            return Ok(responseModel);
        }

        /// <summary>
        /// Get the profile which is realted to the user of the passed id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProfileByUserId/{id}")]
        public async Task<ActionResult<ApiResponse<GetProfileApiModel.Response>>> GetProfileByUserId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var responseModel = new ApiResponse<GetProfileApiModel.Response>();

            if(user == null)
            {
                responseModel.AddError(5);
                return Ok(responseModel);
            }

            var profile = await _dataAccess.Profiles.FirstOrDefaultAsync(profile => profile.UserId == user.Id);

            if (profile != null)
            {
                responseModel.Data = new GetProfileApiModel.Response()
                {
                    Id = profile.Id,
                    Address = profile.Address,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    // TODO: Add the proper rating
                    Rating = 0
                };
            }

            return Ok(responseModel);
        }
    }
}
