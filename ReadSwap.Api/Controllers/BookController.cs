using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private readonly int _maxFileSize = 1 * 1024 * 1024;
        private readonly string _imagesFolderPath = "\\images\\bookCovers\\";

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

        [HttpPost(ApiRoutes.AddBookImage)]
        public async Task<ActionResult<ApiResponse>> AddBookImage(int bookId, IFormFile image)
        {
            var responseModel = new ApiResponse();

            if (image == null)
            {
                responseModel.AddError(9);
                return Ok(responseModel);
            }

            var book = await _dataAccess.Books
                .Include(b=>b.BookImage)
                .Include(b=>b.BookImages)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if(book==null)
            {
                responseModel.AddError(8);
                return Ok(responseModel);
            }

            if(image.ContentType.Contains("image")==false)
            {
                responseModel.AddError(9);
                return Ok(responseModel);
            }

            if(image.Length > _maxFileSize)
            {
                responseModel.AddError(10);
                return Ok(responseModel);
            }

            var ext = Path.GetExtension(image.FileName);

            var name = Guid.NewGuid().ToString();

            var bookImage = new BookImage
            {
                ImageName = name + ext,
            };
            book.BookImages.Add(bookImage);

            if (book.BookImage == null)
            {
                book.BookImage = bookImage;
            }


            var currentDirectory = Directory.GetCurrentDirectory();

            var path = currentDirectory + "\\wwwroot" + _imagesFolderPath ;

            Directory.CreateDirectory(path);

            using(var fileStream = new FileStream(path + name + ext,FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            await _dataAccess.SaveChangesAsync();

            return Ok(responseModel);
        }

    }
}
