using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGLClientBase.Models
{
    public class ProductFinal
    {
        public int ID { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(40)]
        public string Codename { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public double DollarPrice { get; set; }

        [Required]
        public string ImagePath { get; set; }
    }
}