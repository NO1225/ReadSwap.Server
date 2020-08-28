using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class EditProfileApiModel
    {
        public class Request
        {
            /// <summary>
            /// The first name of the user
            /// </summary>
            /// <example>Ali</example>
            [MaxLength(50)]
            public string FirstName { get; set; }

            /// <summary>
            /// The last name of the user
            /// </summary>
            /// <example>Hussain</example>
            [MaxLength(50)]
            public string LastName { get; set; }

            /// <summary>
            /// The address of the user
            /// </summary>
            /// <example>Amman-Jordan</example>
            [MaxLength(200)]
            public string Address { get; set; }

        }

        public class Response
        {
            /// <summary>
            /// The id of this profile
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// The first name of the user
            /// </summary>
            /// <example>Ali</example>
            public string FirstName { get; set; }

            /// <summary>
            /// The last name of the user
            /// </summary>
            /// <example>Hussain</example>
            public string LastName { get; set; }

            /// <summary>
            /// The address of the user
            /// </summary>
            /// <example>Amman-Jordan</example>
            public string Address { get; set; }

            /// <summary>
            /// The rating of the user
            /// </summary>
            public double Rating { get; set; }
        }
    }
}
