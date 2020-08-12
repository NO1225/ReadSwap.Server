using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class SignUpApiModel
    {
        public class Request
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MinLength(8)]
            [MaxLength(20)]
            public string Passward { get; set; }

        }

        public class Response
        {
            public string Email { get; set; }

        }
    }
}
