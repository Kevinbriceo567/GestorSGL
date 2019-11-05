﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGLClientBase.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(9)]
        public string Rut { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailCompany { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Range(1000, 50000, ErrorMessage = "Value must be between {1} and {2}.")]
        [Required]
        public string DollarSalary { get; set; }

        [StringLength(25)]
        [Required]
        public string Position { get; set; }


    }
}