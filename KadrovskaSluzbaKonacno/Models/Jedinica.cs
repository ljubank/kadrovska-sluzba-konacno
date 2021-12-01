using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KadrovskaSluzbaKonacno.Models
{
    public class Jedinica
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Ime { get; set; }

        [Range(2010, 2019)]
        public int GodinaOsnivanja { get; set; }
    }
}