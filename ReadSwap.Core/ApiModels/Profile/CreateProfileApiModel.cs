using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class CreateProfileApiModel
    {
        public class Request
        {
            /// <summary>
            /// The first name of the user
            /// </summary>
            /// <example>Ali</example>
            [MaxLength(50)]
            [Required]
            public string FirstName { get; set; }

            /// <summary>
            /// The last name of the user
            /// </summary>
            /// <example>Hussain</example>
            [MaxLength(50)]
            [Required]
            public string LastName { get; set; }

            /// <summary>
            /// The address of the user
            /// </summary>
            /// <example>Amman-Jordan</example>
            [MaxLength(200)]
            [Required]
            public string Address { get; set; }

        }

    }
}
