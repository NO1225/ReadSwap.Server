using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class CheckEmailApiModel
    {
        public class Request
        {
            /// <summary>
            /// The email to check
            /// </summary>
            [EmailAddress]
            public string Email { get; set; }
        }

        public class Response
        {

            /// <summary>
            /// Wiether the email already registered or not
            /// </summary>
            public bool Exists { get; set; }
        }
    }
}
