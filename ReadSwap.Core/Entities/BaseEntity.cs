using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReadSwap.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool Disabled { get; set; } = false;

        public DateTime? DisabledOn { get; set; } = null;
    }
}
