using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGLClientBase.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(30)]
        public string Codename { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate  { get; set; }

        /*
        [DataType(DataType.EmailAddress)]
        public string EmailCompany { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [StringLength(25)]
        [Required]
        public string Position { get; set; }
        [Range(1000, 50000, ErrorMessage = "Value must be between {1} and {2}.")]
        */

        [Required]
        public string DollarPrice { get; set; }
    }
}