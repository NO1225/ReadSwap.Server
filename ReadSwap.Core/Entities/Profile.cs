using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReadSwap.Core.Entities
{
    public class Profile:BaseEntity
    {
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        [ForeignKey(nameof(User))]
        [MaxLength(450)]
        [Required]
        public string UserId { get; set; }

        public AppUser User { get; set; }
    }
}
