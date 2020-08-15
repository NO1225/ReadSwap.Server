using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class TokenApiModel
    {
        public class Request
        {
            /// <summary>
            /// The expired access token
            /// </summary>
            [Required]
            public string AccessToken { get; set; }

            /// <summary>
            /// The refresh token to be used to renew the access token
            /// </summary>
            [Required]
            public string RefreshToken { get; set; }
        }

        public class Response
        {
            /// <summary>
            /// The new access token
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// New one time use refresh token
            /// </summary>
            public string RefreshToken { get; set; }
        }
    }
}
