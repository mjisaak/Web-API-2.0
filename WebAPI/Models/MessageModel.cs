using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Range(0, 100)]
        [Required]
        public int Age { get; set; }
    }
}