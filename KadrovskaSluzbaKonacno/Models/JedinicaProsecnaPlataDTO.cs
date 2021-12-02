using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KadrovskaSluzbaKonacno.Models
{
    public class JedinicaProsecnaPlataDTO
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Ime { get; set; }

        public decimal ProsecnaPlata { get; set; }
    }
}