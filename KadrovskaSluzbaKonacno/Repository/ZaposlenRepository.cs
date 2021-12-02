using KadrovskaSluzbaKonacno.Interfaces;
using KadrovskaSluzbaKonacno.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace KadrovskaSluzbaKonacno.Repository
{
    public class ZaposlenRepository : IDisposable, IZaposlenRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

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

        public IEnumerable<Zaposlen> GetAll()
        {
            return db.Zaposleni.Include(z => z.Jedinica).OrderBy(z => z.GodinaZaposlenja);
        }

        public Zaposlen GetById(int id)
        {
            return db.Zaposleni.Include(j => j.Jedinica).FirstOrDefault(j => j.Id == id);
        }

        public IEnumerable<Zaposlen> GetByGodina(int rodjenje)
        {
            IEnumerable<Zaposlen> result = db.Zaposleni.Include(j => j.Jedinica).Where(j => j.GodinaRodjenja > rodjenje).OrderBy(j => j.GodinaRodjenja);
            return result;
        }

        public void Add(Zaposlen zaposlen)
        {
            db.Zaposleni.Add(zaposlen);
            db.SaveChanges();
        }

        public void Update(Zaposlen zaposlen)
        {
            db.Entry(zaposlen).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }
        }

        public void Delete(Zaposlen zaposlen)
        {
            db.Zaposleni.Remove(zaposlen);
            db.SaveChanges();
        }

        public IEnumerable<Zaposlen> GetByPlata(ZaposlenPlata zaposlenPlata)
        {
            IEnumerable<Zaposlen> result = db.Zaposleni.Include(j => j.Jedinica).Where(z => z.Plata >= zaposlenPlata.Najmanje && z.Plata <= zaposlenPlata.Najvise).OrderByDescending(z => z.Plata);
            return result;
        }

        public IEnumerable<JedinicaProsecnaPlataDTO> GetJediniceByProsecnaPlata(decimal granica)
        {
            IEnumerable<Zaposlen> zaposleni = GetAll();
            var result = zaposleni.GroupBy(
                z => z.Jedinica,
                z => z.Plata,
                (jedinica, prosecnaPlata) => new JedinicaProsecnaPlataDTO()
                {
                    Id = jedinica.Id,
                    Ime = jedinica.Ime,
                    ProsecnaPlata = prosecnaPlata.Average()
                }).Where(j => j.ProsecnaPlata > granica).OrderBy(j => j.ProsecnaPlata).AsEnumerable();

            return result;
        }
    }
}