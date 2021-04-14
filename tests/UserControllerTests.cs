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
            Credentials credentials2 = new Credentials() { Userid = 2, Password = "123", PasswordConfirmed = "123" };
            Credentials credentials3 = new Credentials() { Userid = 3, Password = "123", PasswordConfirmed = "123" };
            Usertype type = new Usertype() { Typeid = 1, Name = "user" };
            PageUser user1 = new PageUser { Userid = 1, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test1@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials };
            PageUser user2 = new PageUser { Userid = 2, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test2@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials2 };
            PageUser user3 = new PageUser { Userid = 3, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test3@user.com", emailConfirmation = false, isBlocked = false, isReported = false, Type = type, Credentials = credentials3 };
            context.Usertype.Add(type);
            context.PageUser.Add(user1);
            context.PageUser.Add(user2);
            context.PageUser.Add(user3);
            context.Credentials.Add(credentials);
            context.Credentials.Add(credentials2);
            context.Credentials.Add(credentials3);
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
            PageUser user = new PageUser { Userid = 1, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test1@user.com" };

            // Act
            var result = userController.Register(user).Result as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewData["ErrorEmail"], "Podany adres email jest juz zajety, proszę podac inny");
        }

        [TestMethod]
        public void BlockUserTest()
        {
            // Act
            var result = userController.BlockUser(1, 1).Result as ViewResult;
            List<PageUser> users = (userController.List().Result as ViewResult).Model as List<PageUser>;

            //Assert
            Assert.IsTrue(users[0].isBlocked);
            Assert.AreEqual(1, users[0].blockType);
        }

        [TestMethod]
        public void RejectReportTest()
        {
            // Arrange
            List<PageUser> users = (userController.List().Result as ViewResult).Model as List<PageUser>;
            users[1].isReported = true;
            context.Update(users[1]);
            context.SaveChangesAsync();

            //Assert
            Assert.IsTrue(users[1].isReported);

            //Then
            var result = userController.RejectReport(2).Result as ViewResult;
            users = (userController.List().Result as ViewResult).Model as List<PageUser>;

            //Assert
            Assert.IsFalse(users[1].isReported);
        }

        [TestMethod]
        public void UserDetailsTest()
        {
            // Act
            var result = userController.Details(1).Result as ViewResult;
            PageUser user = result.Model as PageUser;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test1@user.com", user.EmailAddress);
        }

        [TestMethod]
        public void NullUserDetailsTest()
        {
            // Act
            var result = userController.Details(null).Result as ViewResult;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UserNotExistsDetailsTest()
        {
            // Act
            var result = userController.Details(12).Result as ViewResult;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            // Act
            var result = userController.Delete(2).Result as ViewResult;
            result = userController.Details(2).Result as ViewResult;
            List<PageUser> users = (userController.List().Result as ViewResult).Model as List<PageUser>;

            //Assert
            Assert.IsNull(result);
            Assert.AreEqual(2, users.Count);
        }

        [TestMethod]
        public void DeleteUserWithOffersTest()
        {
            //Arrange
            Offer offer1 = new Offer { Offerid = 1, UserId = 1, CategoryId = 1, Title = "1" };
            Offer offer2 = new Offer { Offerid = 2, UserId = 1, CategoryId = 1, Title = "2" };
            context.Add(offer1);
            context.Add(offer2);
            context.SaveChangesAsync();

            //Act
            List<Offer> offers = context.Offer.ToListAsync().Result as List<Offer>;

            //Assert
            Assert.AreEqual(2, offers.Count);

            //Then
            var result = userController.Delete(1).Result as ViewResult;
            result = userController.Details(1).Result as ViewResult;

            //Assert
            offers = context.Offer.ToListAsync().Result as List<Offer>;
            Assert.AreEqual(0, offers.Count);
        }

        [TestMethod]
        public void NullUserDeleteTest()
        {
            // Act
            var result = userController.Delete(null).Result as ViewResult;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UserNotExistsDeleteTest()
        {
            // Act
            var result = userController.Delete(12).Result as ViewResult;

            //Assert
            Assert.IsNull(result);
        }

        // [TestMethod]
        // public void UserLoginTest()
        // {
        //     // Act
        //     var result = userController.Login("test1@user.com", "123", "").Result as ViewResult;

        //     //Assert
        //     Assert.IsNull(result);
        // }

        [TestMethod]
        public void UserEmailNotConfirmedLogin()
        {
            // Act
            var result = userController.Login("test3@user.com", "123", "").Result as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Email nie został potwierdzony", result.ViewData["error"]);
        }

        [TestMethod]
        public void NullUserLoginTest()
        {
            // Act
            var result = userController.Login(".@user.com", "", "").Result as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Podana nazwa użytkownika nie istnieje", result.ViewData["error"]);
        }

        // [TestMethod]
        // public void UserInvalidPasswordLoginTest()
        // {
        //     // Act
        //     var result = userController.Login("test1@user.com", "abc", "").Result as ViewResult;

        //     //Assert
        //     Assert.IsNotNull(result);
        //     Assert.AreEqual("Podano złe hasło", result.ViewData["error"]);
        // }
    }
}
