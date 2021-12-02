using KadrovskaSluzbaKonacno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadrovskaSluzbaKonacno.Interfaces
{
    public interface IZaposlenRepository
    {
        IEnumerable<Zaposlen> GetAll();
        Zaposlen GetById(int id);
        IEnumerable<Zaposlen> GetByGodina(int godina);
        void Add(Zaposlen zaposlen);
        void Update(Zaposlen zaposlen);
        void Delete(Zaposlen zaposlen);
        IEnumerable<Zaposlen> GetByPlata(ZaposlenPlata zaposlenPlata);
        IEnumerable<JedinicaProsecnaPlataDTO> GetJediniceByProsecnaPlata(decimal granica);
    }
}
