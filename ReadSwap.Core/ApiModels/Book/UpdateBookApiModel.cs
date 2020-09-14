using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class UpdateBookApiModel
    {
        public class Request
        {

            public int BookId { get; set; }

            /// <summary>
            /// The title of the book
            /// </summary>
            [MaxLength(200)]
            [Required]
            public string Title { get; set; }

            /// <summary>
            /// The condition of the book
            /// </summary>
            [Required]
            [Range(1, 10)]
            public int Condition { get; set; }

            /// <summary>
            /// The author of the book
            /// </summary>
            [MaxLength(50)]
            public string Author { get; set; }

            /// <summary>
            /// The publish year of the book
            /// </summary>
            public ushort Year { get; set; }

            /// <summary>
            /// Desciption about the book
            /// </summary>
            [MaxLength(400)]
            public string Description { get; set; }


        }

    }
}
