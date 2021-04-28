using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using freelancerzy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using freelancerzy.Controllers;
using Moq;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace tests
{
    [TestClass]
    public class OffersControllerTests
    {
        private cb2020freedbContext context;
        private OffersController controller;

        [TestInitialize]
        public void Startup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<cb2020freedbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new cb2020freedbContext(optionsBuilder.Options);
            controller = new OffersController(context);

            TestData.InitializeOffer(context);
            context.SaveChangesAsync();
        }

        [TestMethod]
        public void IndexRedirectTest()
        {
            // Act
            var result = controller.Index();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual(((RedirectToActionResult)result).ActionName, "Search");
        }

        [TestMethod]
        public void SearchViewTest()
        {
            // Act
            var result = controller.Search();

            var model = (result as ViewResult).Model;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DetailsTest()
        {
            // Act
            var result = controller.Details(1).Result as ViewResult;

            Offer model = result.Model as Offer;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.CategoryId, 1);
            Assert.AreEqual(model.UserId, 1);
            Assert.AreEqual(model.Title, "AAA");
        }

        [TestMethod]
        public void DetailsNullOfferTest()
        {
            // Act
            var result = controller.Details(null).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DetailsOfferNotExistTest()
        {
            // Act
            var result = controller.Details(12).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteUnauthorizeTest()
        {
            // Act
            var result = controller.DeleteConfirmed(1).Result as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void DeleteOfferNotExistTest()
        {
            // Act
            var result = controller.DeleteConfirmed(12).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
