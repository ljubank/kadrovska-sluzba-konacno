using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KadrovskaSluzbaKonacno.Models
{
    public class Zaposlen
    {
        public int Id { get; set; }

        [MaxLength(70)]
        [Required]
        public string ImeIPrezime { get; set; }

        [MaxLength(50)]
        [Required]
        public string Rola { get; set; }

        [Range(1960, 1999)]
        public int GodinaRodjenja { get; set; }

        [Range(2010, 2019)]
        [Required]
        public int GodinaZaposlenja { get; set; }

        [Range(250.1, 9999.9)]
        [Required]
        public decimal Plata { get; set; }

        // Foreign key
        public int JedinicaId { get; set; }
        // Navigation attribute
        public Jedinica Jedinica { get; set; }
    }
}