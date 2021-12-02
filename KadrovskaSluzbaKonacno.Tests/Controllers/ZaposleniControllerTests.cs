using KadrovskaSluzbaKonacno.Controllers;
using KadrovskaSluzbaKonacno.Interfaces;
using KadrovskaSluzbaKonacno.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace KadrovskaSluzbaKonacno.Tests.Controllers
{
    [TestClass]
    public class ZaposleniControllerTests
    {
        [TestMethod]
        public void GetReturnsZaposlenWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IZaposlenRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Zaposlen { Id = 1, ImeIPrezime = "Ljuban Knezevic", Rola = "Software developer", GodinaRodjenja = 1995, GodinaZaposlenja = 2010, Plata = 500, JedinicaId = 3 });

            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Zaposlen>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IZaposlenRepository>();
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(10, new Zaposlen { Id = 1, ImeIPrezime = "Ljuban Knezevic", Rola = "Software developer", GodinaRodjenja = 1995, GodinaZaposlenja = 2010, Plata = 500, JedinicaId = 3 });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<Zaposlen> products = new List<Zaposlen>();
            products.Add(new Zaposlen { Id = 1, ImeIPrezime = "Ljuban Knezevic", Rola = "Software developer", GodinaRodjenja = 1990, GodinaZaposlenja = 2010, Plata = 500, JedinicaId = 3 });
            products.Add(new Zaposlen { Id = 2, ImeIPrezime = "Marko Markovic", Rola = "Software developer", GodinaRodjenja = 1985, GodinaZaposlenja = 2006, Plata = 1000, JedinicaId = 3 });

            var mockRepository = new Mock<IZaposlenRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(products.AsEnumerable().OrderBy(z => z.GodinaZaposlenja));
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IEnumerable<Zaposlen> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products.Count, result.ToList().Count);
            Assert.AreEqual(products.ElementAt(0), result.ElementAt(1));
            Assert.AreEqual(products.ElementAt(1), result.ElementAt(0));
        }

        [TestMethod]
        public void PostReturnsMultipleObjects()
        {
            // Arrange
            List<Zaposlen> products = new List<Zaposlen>();
            products.Add(new Zaposlen { Id = 1, ImeIPrezime = "Ljuban Knezevic", Rola = "Software developer", GodinaRodjenja = 1990, GodinaZaposlenja = 2010, Plata = 500, JedinicaId = 3 });
            products.Add(new Zaposlen { Id = 2, ImeIPrezime = "Marko Markovic", Rola = "Software developer", GodinaRodjenja = 1985, GodinaZaposlenja = 2006, Plata = 1000, JedinicaId = 3 });
            products.Add(new Zaposlen { Id = 3, ImeIPrezime = "Ivana Ivanovic", Rola = "Software developer", GodinaRodjenja = 1992, GodinaZaposlenja = 2008, Plata = 1500, JedinicaId = 3 });

            ZaposlenPlata zaposlenPlata = new ZaposlenPlata { Najmanje = 700, Najvise = 1600 };

            var mockRepository = new Mock<IZaposlenRepository>();
            mockRepository.Setup(x => x.GetByPlata(zaposlenPlata)).Returns(products.AsEnumerable().Where(z => z.Plata >= zaposlenPlata.Najmanje && z.Plata <= zaposlenPlata.Najvise).OrderByDescending(z => z.Plata));
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IEnumerable<Zaposlen> result = controller.PostByPlata(zaposlenPlata);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products.Count, result.ToList().Count + 1);
            Assert.AreEqual(products.ElementAt(2), result.ElementAt(0));
            Assert.AreEqual(products.ElementAt(1), result.ElementAt(1));
        }
    }
}