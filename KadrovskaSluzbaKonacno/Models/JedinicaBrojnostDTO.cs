using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KadrovskaSluzbaKonacno.Models
{
    public class JedinicaBrojnostDTO
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Ime { get; set; }

        public int Brojnost { get; set; }
    }
}