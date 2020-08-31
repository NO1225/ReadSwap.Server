using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class ChangePasswordApiModel
    {
        /// <summary>
        /// The request of the signing in call
        /// </summary>
        public class Request
        {
            /// <summary>
            /// The old password
            /// </summary>
            [Required]
            public string OldPassword { get; set; }

            /// <summary>
            /// the new password to replace the old on
            /// </summary>
            /// <example>string123</example>
            [Required]
            [MinLength(8)]
            [MaxLength(20)]
            public string NewPassward { get; set; }

        }

    }
}
