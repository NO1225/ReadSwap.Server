using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

namespace ReadSwap.Core.Entities
{
    public class Book:BaseEntity
    {
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        
        [Required]
        [Range(1,10)]
        public int Condition { get; set; }

        [MaxLength(50)]
        public string Author { get; set; }

        public ushort Year { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        [AllowNull]
        public int? BookImageId { get; set; }

        public BookImage BookImage { get; set; }

        public ICollection<BookImage> BookImages { get; set; }

        [Required]
        [MaxLength(450)]
        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
