using System;
using freelancerzy.Models;
using Microsoft.AspNetCore.Identity;

namespace tests
{
    public class TestData
    {
        private static Credentials credentials1, credentials2, credentials3;
        private static Usertype type;
        private static PageUser user1, user2, user3;
        private static Offer offer1, offer2, offer3;
        private static Category category1, category2, category3;
        public static void InitializeCredentials(cb2020freedbContext context)
        {
            var passwordHasher = new PasswordHasher<string>();
            credentials1 = new Credentials() { Userid = 1, Password = passwordHasher.HashPassword("test1@user.com", "123") };
            credentials2 = new Credentials() { Userid = 2, Password = passwordHasher.HashPassword("test2@user.com", "123") };
            credentials3 = new Credentials() { Userid = 3, Password = passwordHasher.HashPassword("test3@user.com", "123") };

            context.Credentials.Add(credentials1);
            context.Credentials.Add(credentials2);
            context.Credentials.Add(credentials3);
        }

        public static void InitializeUserType(cb2020freedbContext context)
        {
            type = new Usertype() { Typeid = 1, Name = "user" };
            context.Usertype.Add(type);
        }
        public static void InitializeUsers(cb2020freedbContext context)
        {
            InitializeCredentials(context);
            InitializeUserType(context);
            user1 = new PageUser { Userid = 1, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test1@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials1 };
            user2 = new PageUser { Userid = 2, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test2@user.com", emailConfirmation = true, isBlocked = false, isReported = false, Type = type, Credentials = credentials2 };
            user3 = new PageUser { Userid = 3, TypeId = 1, FirstName = "John", Surname = "Doe", EmailAddress = "test3@user.com", emailConfirmation = false, isBlocked = false, isReported = false, Type = type, Credentials = credentials3 };

            context.PageUser.Add(user1);
            context.PageUser.Add(user2);
            context.PageUser.Add(user3);
        }

        public static void InitializeCategory(cb2020freedbContext context)
        {
            category1 = new Category { Categoryid = 1, CategoryName = "abc" };
            category2 = new Category { Categoryid = 2, CategoryName = "cbd" };
            category3 = new Category { Categoryid = 3, CategoryName = "xyz" };
            context.Category.Add(category1);
            context.Category.Add(category2);
            context.Category.Add(category3);
        }
        public static void InitializeOffer(cb2020freedbContext context)
        {
            InitializeUsers(context);
            InitializeCategory(context);

            offer1 = new Offer { Offerid = 1, UserId = 1, CategoryId = 1, Title = "AAA", Description = "AAA", CreationDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(14), ViewCounter = 0, Wage = 1, IsReported = false };
            offer2 = new Offer { Offerid = 2, UserId = 1, CategoryId = 1, Title = "BBB", Description = "BBB", CreationDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(14), ViewCounter = 0, Wage = 1, IsReported = false };
            offer3 = new Offer { Offerid = 3, UserId = 1, CategoryId = 1, Title = "CCC", Description = "CCC", CreationDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(14), ViewCounter = 0, Wage = 1, IsReported = false };
            context.Offer.Add(offer1);
            context.Offer.Add(offer2);
            context.Offer.Add(offer3);
        }
    }
}