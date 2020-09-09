using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class SignUpApiModel
    {
        /// <summary>
        /// The request for signing up call
        /// </summary>
        public class Request
        {
            /// <summary>
            /// The email of the new account
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            /// The Password of the new account
            /// </summary>
            [Required]
            [MinLength(8)]
            [MaxLength(20)]
            public string Password { get; set; }

        }

        /// <summary>
        /// The response of signing up call
        /// </summary>
        public class Response
        {
            /// <summary>
            /// The email of the new account
            /// </summary>
            public string Email { get; set; }

        }
    }
}
