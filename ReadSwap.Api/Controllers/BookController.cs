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

        /// <summary>
        /// Adding book to this user collection
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.UpdateBook)]
        public async Task<ActionResult<ApiResponse>> UpdateBook(UpdateBookApiModel.Request requestModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var book = await _dataAccess.Books.Where(b => b.Disabled != true && b.AppUserId == user.Id && b.Id == requestModel.BookId).FirstOrDefaultAsync();
            var responseModel = new ApiResponse();

            if (book == null)
            {
                responseModel.AddError(8);
                return Ok(responseModel);
            }

            book.Author = requestModel.Author;
            book.Condition = requestModel.Condition;
            book.Description = requestModel.Description;
            book.Title = requestModel.Title;
            book.Year = requestModel.Year;

            await _dataAccess.SaveChangesAsync();

            return Ok(responseModel);
        }


        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpDelete(ApiRoutes.DeleteBook)]
        public async Task<ActionResult<ApiResponse>> DeleteBook(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            var book = await _dataAccess.Books.Where(b => b.Disabled != true && b.AppUserId == user.Id && b.Id == bookId).FirstOrDefaultAsync();
            var responseModel = new ApiResponse();

            if (book == null)
            {
                responseModel.AddError(8);
                return Ok(responseModel);
            }

            book.Disabled = true;
            book.DisabledOn = DateTime.UtcNow;

            await _dataAccess.SaveChangesAsync();

            return Ok(responseModel);
        }

        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.GetMyBooks)]
        public async Task<ActionResult<ApiResponse<GetBooksApiModel.Response>>> GetMyBooks()
        {
            var responseModel = new ApiResponse<GetBooksApiModel.Response>();

            var user = await _userManager.GetUserAsync(User);

            responseModel.Data = new GetBooksApiModel.Response() {
                Books = await _dataAccess.Books.
                Where(b => b.Disabled != true && b.AppUserId == user.Id)
                .Include(b => b.BookImage)
                .Select(b => new GetBooksApiModel.Book
                {
                    Author = b.Author,
                    BookId = b.Id,
                    Condition = b.Condition,
                    CoverPath = $"{_imagesFolderPath}{b.BookImage.ImageName}",
                    Description = b.Description,
                    Title = b.Title,
                    Year = b.Year
                }).ToListAsync(),
            };

            return Ok(responseModel);
        }

        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.GetBookById)]
        public async Task<ActionResult<ApiResponse<GetBookApiModel.Response>>> GetBookById(int bookId)
        {
            var responseModel = new ApiResponse<GetBookApiModel.Response>();

            var book = await _dataAccess.Books.Include(b => b.BookImage).Include(b => b.BookImages).FirstOrDefaultAsync(b => b.Disabled != true && b.Id == bookId);

            if (book == null)
            {
                responseModel.AddError(8);
                return Ok(responseModel);
            }

            responseModel.Data = new GetBookApiModel.Response()
            {
                Author = book.Author,
                BookId = book.Id,
                Condition = book.Condition,
                Description = book.Description,
                Title = book.Title,
                Year = book.Year,
                BookImages = book.BookImages.Select(bi => new GetBookApiModel.BookImage
                {
                    BookImageId = bi.Id,
                    ImagePath = $"{_imagesFolderPath}{bi.ImageName}",
                    IsCover = bi.Id == book.BookImageId
                }).ToList()
            };

            return Ok(responseModel);
        }

        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.SearchBookByTitle)]
        public async Task<ActionResult<ApiResponse<GetBooksApiModel.Response>>> SearchBookByTitle(string bookTitle)
        {
            var responseModel = new ApiResponse<GetBooksApiModel.Response>();

            var normalizedBookTitle = bookTitle.ToUpper();

            responseModel.Data = new GetBooksApiModel.Response()
            {
                Books = await _dataAccess.Books.
                Where(b => b.Disabled != true && b.NormalizedTitle.Contains(normalizedBookTitle))
                .Include(b => b.BookImage)
                .Select(b => new GetBooksApiModel.Book
                {
                    Author = b.Author,
                    BookId = b.Id,
                    Condition = b.Condition,
                    CoverPath = $"{_imagesFolderPath}{b.BookImage.ImageName}",
                    Description = b.Description,
                    Title = b.Title,
                    Year = b.Year
                }).ToListAsync(),
            };

            return Ok(responseModel);
        }

        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
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
                .Include(b => b.BookImage)
                .Include(b => b.BookImages)
                .FirstOrDefaultAsync(b => b.Disabled != true && b.Id == bookId);

            if (book == null)
            {
                responseModel.AddError(8);
                return Ok(responseModel);
            }

            if (image.ContentType.Contains("image") == false)
            {
                responseModel.AddError(9);
                return Ok(responseModel);
            }

            if (image.Length > _maxFileSize)
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

            var path = currentDirectory + "\\wwwroot" + _imagesFolderPath;

            Directory.CreateDirectory(path);

            using (var fileStream = new FileStream(path + name + ext, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            await _dataAccess.SaveChangesAsync();

            return Ok(responseModel);
        }

        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.ChangeBookCover)]
        public async Task<ActionResult<ApiResponse>> ChangeBookCover(ChangeBookCoverApiModel.Request requestModel)
        {
            var responseModel = new ApiResponse();

            var user = await _userManager.GetUserAsync(User);

            var book = await _dataAccess.Books
                .Include(b => b.BookImage)
                .Include(b => b.BookImages)
                .FirstOrDefaultAsync(b => b.Disabled != true && (b.Id == requestModel.BookId && b.AppUserId == user.Id)&&b.BookImages.Select(bi=>bi.Id).Contains(requestModel.BookImageId));

            if (book == null)
            {
                responseModel.AddError(8);
                return Ok(responseModel);
            }

            book.BookImageId = requestModel.BookImageId;
            await _dataAccess.SaveChangesAsync();

            return Ok(responseModel);
        }

        /// <summary>
        /// Add Image to the collection of a book images
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpDelete(ApiRoutes.DeleteBookImage)]
        public async Task<ActionResult<ApiResponse>> DeleteBookImage(int bookImageId)
        {
            var responseModel = new ApiResponse();

            var user = await _userManager.GetUserAsync(User);

            var bookImage = await _dataAccess.BookImages
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => (b.Id == bookImageId && b.Book.AppUserId == user.Id)&& bookImageId != b.Book.BookImageId);

            if (bookImage == null)
            {
                responseModel.AddError(11);
                return Ok(responseModel);
            }

            var pathToFile = Directory.GetCurrentDirectory() + "\\wwwroot" + _imagesFolderPath + bookImage.ImageName;

            System.IO.File.Delete(pathToFile);

            _dataAccess.BookImages.Remove(bookImage);
            await _dataAccess.SaveChangesAsync();

            return Ok(responseModel);
        }



    }
}
