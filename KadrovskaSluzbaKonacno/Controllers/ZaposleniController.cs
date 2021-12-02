using KadrovskaSluzbaKonacno.Interfaces;
using KadrovskaSluzbaKonacno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KadrovskaSluzbaKonacno.Controllers
{
    public class ZaposleniController : ApiController
    {
        IZaposlenRepository _repository { get; set; }

        public ZaposleniController(IZaposlenRepository repository)
        {
            _repository = repository;
        }

        // GET api/zaposleni
        public IEnumerable<Zaposlen> Get()
        {
            return _repository.GetAll();
        }

        // GET api/zaposleni/{id}
        public IHttpActionResult Get(int id)
        {
            var zaposlen = _repository.GetById(id);
            if (zaposlen == null)
            {
                return NotFound();
            }

            return Ok(zaposlen);
        }

        // GET api/zaposleni?rodjenje={godina}
        public IEnumerable<Zaposlen> GetByGodina(int rodjenje)
        {
            return _repository.GetByGodina(rodjenje);
        }

        // POST api/zaposleni
        [Authorize]
        public IHttpActionResult Post(Zaposlen zaposlen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(zaposlen);
            return CreatedAtRoute("DefaultApi", new { id = zaposlen.Id }, zaposlen);
        }

        // PUT api/zaposleni/{id}
        public IHttpActionResult Put(int id, Zaposlen zaposlen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != zaposlen.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(zaposlen);
            }
            catch
            {
                throw;
            }

            return Ok(zaposlen);
        }

        // DELETE api/zaposleni/{id}
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            var zaposlen = _repository.GetById(id);
            if (zaposlen == null)
            {
                return NotFound();
            }

            _repository.Delete(zaposlen);
            return Ok();
        }

        // POST api/pretraga
        [Authorize]
        [Route("api/pretraga")]
        public IEnumerable<Zaposlen> PostByPlata(ZaposlenPlata zaposlenPlata)
        {
            return _repository.GetByPlata(zaposlenPlata);
        }

        // POST api/plate?granica={prosecnaPlata}
        [Route("api/plate")]
        public IEnumerable<JedinicaProsecnaPlataDTO> PostJediniceByProsecnaPlata(decimal granica)
        {
            return _repository.GetJediniceByProsecnaPlata(granica);
        }
    }
}
