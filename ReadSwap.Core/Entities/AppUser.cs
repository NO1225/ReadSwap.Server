using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.Entities
{
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// The current refresh token of this user
        /// </summary>
        [MaxLength(50)]
        public string RefreshToken { get; set; }
    }
}
