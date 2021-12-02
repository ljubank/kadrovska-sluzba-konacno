using KadrovskaSluzbaKonacno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadrovskaSluzbaKonacno.Interfaces
{
    public interface IJedinicaRepository
    {
        IEnumerable<Jedinica> GetAll();
        Jedinica GetById(int id);
        IEnumerable<Jedinica> GetByTradicija();
        IEnumerable<JedinicaBrojnostDTO> GetByBrojnost();
    }
}
