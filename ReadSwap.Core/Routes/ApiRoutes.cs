using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ReadSwap.Core.Routes
{
    public static class ApiRoutes
    {

        #region App Version

        public const string ErrorLibrary = "api/AppVersion/ErrorLibrary";

        #endregion

        #region Auth

        public const string CheckEmailExists = "api/Auth/CheckEmailExists";

        public const string SignUp = "api/Auth/SignUp";

        public const string SignIn = "api/Auth/SignIn";

        public const string ChangePassword = "api/Auth/ChangePassword";

        #endregion

        #region Token

        public const string Refresh = "api/Token/Refresh";

        public const string Revoke = "api/Token/Revoke";

        #endregion

        #region Profile

        public const string CreateMyProfile = "api/Profile/CreateMyProfile";

        public const string GetMyProfile = "api/Profile/GetMyProfile";

        public const string EditMyProfile = "api/Profile/EditMyProfile";

        public const string GetProfile = "api/Profile/GetProfile/{id}";

        public const string GetProfileByUserId = "api/Profile/GetProfileByUserId/{id}";

        #endregion

        #region Book

        public const string AddBook = "api/Book/Add";

        public const string AddBookImage = "api/Book/AddImage/{bookId}";

        public const string GetBookById = "api/Book/GetById/{bookId}";

        public const string GetMyBooks = "api/Book/GetMyBooks";

        public const string ChangeBookCover = "api/Book/ChangeCover";

        #endregion
    }
}
