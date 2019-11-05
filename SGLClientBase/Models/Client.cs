using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGLClientBase.Models
{
    public class Client
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [StringLength(25)]
        [Required]
        public string Direction { get; set; }

        [Required]
        [StringLength(9)]
        public string Rut { get; set; }

        [StringLength(40)]
        [DataType(DataType.MultilineText)]
        public string Observation { get; set; }
    }
}