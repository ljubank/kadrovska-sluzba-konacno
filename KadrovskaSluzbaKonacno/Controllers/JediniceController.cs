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
    public class JediniceController : ApiController
    {
        IJedinicaRepository _repository { get; set; }

        public JediniceController(IJedinicaRepository repository)
        {
            _repository = repository;
        }

        // GET api/jedinice
        public IEnumerable<Jedinica> Get()
        {
            return _repository.GetAll();
        }

        // GET api/jedinice/{id}
        public IHttpActionResult Get(int id)
        {
            var jedinica = _repository.GetById(id);
            if (jedinica == null)
            {
                return NotFound();
            }

            return Ok(jedinica);
        }

        // GET api/tradicija
        [Route("api/tradicija")]
        public IEnumerable<Jedinica> GetByTradicija()
        {
            return _repository.GetByTradicija();
        }

        // GET api/brojnost
        [Route("api/brojnost")]
        public IEnumerable<JedinicaBrojnostDTO> GetJediniceByBrojnost()
        {
            return _repository.GetByBrojnost();
        }
    }
}
