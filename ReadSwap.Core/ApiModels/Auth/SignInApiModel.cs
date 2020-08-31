using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class SignInApiModel
    {
        /// <summary>
        /// The request of the signing in call
        /// </summary>
        public class Request
        {
            /// <summary>
            /// The email address of the account
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            /// The passward of the account
            /// </summary>
            /// <example>string123</example>
            [Required]
            public string Passward { get; set; }

        }

        /// <summary>
        /// The response onf the signing in call
        /// </summary>
        public class Response
        {
            /// <summary>
            /// The token for authantication
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// One time use token to renew the access token
            /// </summary>
            public string RefreshToken { get; set; }

        }
    }
}
