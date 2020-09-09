using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadSwap.Core.Entities
{
    public class BookImage : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string ImageName { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
