using KadrovskaSluzbaKonacno.Interfaces;
using KadrovskaSluzbaKonacno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KadrovskaSluzbaKonacno.Repository
{
    public class JedinicaRepository : IDisposable, IJedinicaRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        ZaposlenRepository zp = new ZaposlenRepository();

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Jedinica> GetAll()
        {
            return db.Jedinice;
        }

        public Jedinica GetById(int id)
        {
            return db.Jedinice.FirstOrDefault(j => j.Id == id);
        }

        public IEnumerable<Jedinica> GetByTradicija()
        {
            var result = db.Jedinice.OrderBy(j => j.GodinaOsnivanja).AsEnumerable();

            List<Jedinica> resultFinal = new List<Jedinica>();
            resultFinal.Add(result.ElementAt(0));
            resultFinal.Add(result.ElementAt(result.Count() - 1));

            return resultFinal;
        }

        public IEnumerable<JedinicaBrojnostDTO> GetByBrojnost()
        {
            IEnumerable<Zaposlen> zaposleni = zp.GetAll();
            var result = zaposleni.GroupBy(
                z => z.Jedinica,
                z => z.Id,
                (jedinica, brojnost) => new JedinicaBrojnostDTO()
                {
                    Id = jedinica.Id,
                    Ime = jedinica.Ime,
                    Brojnost = brojnost.Count()
                }).OrderByDescending(j => j.Brojnost).AsEnumerable();

            return result;
        }
    }
}