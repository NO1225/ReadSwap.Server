using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class ChangeBookCoverApiModel
    {
        public class Request
        {
            [Required]
            public int BookId { get; set; }

            [Required]
            public int BookImageId { get; set; }


        }


    }
}
