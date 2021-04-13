using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using freelancerzy.Models;
using Microsoft.Extensions.Configuration;
using TokenGenerator.Managers.Interfaces;
using app.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using freelancerzy.Controllers;
using System.Collections.Generic;

namespace tests
{
    [TestClass]
    public class UserControllerTests
    {
        private cb2020freedbContext context;
        private UserController userController;

        [TestInitialize]
        public void Startup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<cb2020freedbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new cb2020freedbContext(optionsBuilder.Options);
            var config = new Mock<IConfiguration>();
            var tokenManager = new Mock<ITokenManager>();
            userController = new UserController(context, config.Object, tokenManager.Object);

            Credentials credentials = new Credentials() { Userid = 1, Password = "123", PasswordConfirmed = "123" };
            Usertype type = new Usertype() { Typeid = 1, Name = "user" };
            PageUser user1 = new PageUser { Userid = 1, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test1@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials };
            PageUser user2 = new PageUser { Userid = 2, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test2@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials };
            PageUser user3 = new PageUser { Userid = 3, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test3@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials };
            context.Usertype.Add(type);
            context.PageUser.Add(user1);
            context.PageUser.Add(user2);
            context.PageUser.Add(user3);
            context.Credentials.Add(credentials);
            context.SaveChangesAsync();
        }

        [TestMethod]
        public void UserListViewTest()
        {
            // Act
            var result = userController.List().Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(string.IsNullOrEmpty((result as ViewResult).ViewName) || (result as ViewResult).ViewName == "List");
        }


        [TestMethod]
        public void UserListProcessingTest()
        {
            // Act
            var result = userController.List().Result as ViewResult;
            List<PageUser> userList = result.Model as List<PageUser>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, userList.Count);
            Assert.AreEqual(2, userList[1].Userid);
        }

        [TestMethod]
        public void CreateUserDifferentPasswordsTest()
        {
            //Arrange
            PageUser user = new PageUser { Userid = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test@user.com" };
            Credentials credentials = new Credentials() { Password = "123", PasswordConfirmed = "test" };
            user.Credentials = credentials;

            // Act
            var result = userController.Register(user).Result as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewData["Error"], "Hasła nie sa taki same");
        }

        [TestMethod]
        public void CreateUserAlreadyExistsTest()
        {
            //Arrange
            PageUser user = new PageUser { Userid = 1, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test1@user.com"};

            // Act
            var result = userController.Register(user).Result as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewData["ErrorEmail"], "Podany adres email jest juz zajety, proszę podac inny");
        }
    }
}
