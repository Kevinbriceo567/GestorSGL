using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGLClientBase.Models
{

    [Table("Document")]
    public class Document
    {

        [Key]
        public int IdDoc { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public virtual string Image { get; set; }
        public virtual string Pdf { get; set; }

    }
}