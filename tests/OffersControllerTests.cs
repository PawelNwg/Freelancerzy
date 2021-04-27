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
            var result = controller.Details(1);

            var model = result;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
