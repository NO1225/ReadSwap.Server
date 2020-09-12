using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class GetBookApiModel
    {
        
        public class Response
        {
            /// <summary>
            /// THe id of the book
            /// </summary>
            public int BookId { get; set; }
            /// <summary>
            /// The title of the book
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// The condition of the book
            /// </summary>
            public int Condition { get; set; }

            /// <summary>
            /// The author of the book
            /// </summary>
            public string Author { get; set; }

            /// <summary>
            /// The publish year of the book
            /// </summary>
            public ushort Year { get; set; }

            /// <summary>
            /// Desciption about the book
            /// </summary>
            public string Description { get; set; }

            

            public List<BookImage> BookImages { get; set; }
        }

        public class BookImage
        {
            public int BookImageId { get; set; }

            /// <summary>
            /// The Url to the image of the book
            /// </summary>
            public string ImagePath { get; set; }

            public bool IsCover { get; set; }
        }



    }
}
