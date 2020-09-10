using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadSwap.Core.ApiModels;
using ReadSwap.Core.Entities;
using ReadSwap.Core.Routes;
using ReadSwap.Data;

namespace ReadSwap.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataAccess _dataAccess;

        public BookController(UserManager<AppUser> userManager, DataAccess dataAccess)
        {
            this._userManager = userManager;
            this._dataAccess = dataAccess;
        }

        /// <summary>
        /// Adding book to this user collection
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.AddBook)]
        public async Task<ActionResult<ApiResponse<AddBookApiModel.Response>>> AddBook(AddBookApiModel.Request requestModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var book = new Book
            {
                AppUserId = user.Id,
                Author = requestModel.Author,
                Condition = requestModel.Condition,
                Description = requestModel.Description,
                Title = requestModel.Title,
                Year = requestModel.Year,
            };

            await _dataAccess.Books.AddAsync(book);
            await _dataAccess.SaveChangesAsync();

            var response = new ApiResponse<AddBookApiModel.Response>();
            response.Data = new AddBookApiModel.Response
            {
                BookId = book.Id
            };

            return Ok(response);
        }
    }
}
