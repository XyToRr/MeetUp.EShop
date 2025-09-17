using Bogus;
using DataAccess.Context;
using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Models.Client;
using MeetUp.EShop.Core.Models.Order;
using MeetUp.EShop.Core.Models.Product;
using MeetUp.EShop.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataSeed
{
    public static class DataSeeder
    {
        public static void Seed(EShopDbContext context)
        {
            //SeedClients(context);
            SeedUsers(context);
            SeedProducts(context);
            //SeedOrders(context);
        }

        private static void SeedUsers(EShopDbContext context)
        {
            if (context.Users.Any())
                return;

            var userFaker = new Faker<User>()
               .RuleFor(u => u.Id, f => Guid.NewGuid())
               .RuleFor(u => u.FirstName, f => f.Person.FirstName)
               .RuleFor(u => u.Email, f => f.Person.Email)
               .RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password()))
               .RuleFor(u => u.LastName, f => f.Person.LastName)
               .RuleFor(u => u.Login, f => f.Person.UserName)
               .RuleFor(u => u.RefreshToken, f => Guid.NewGuid().ToString());

            var users = userFaker.Generate(10);
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static void SeedProducts(EShopDbContext context)
        {
            if (context.Products.Any())
                return;
            var productFaker = new Faker<Product>()
               .RuleFor(p => p.Id, f => f.Random.Guid())
               .RuleFor(p => p.Code, f => f.Commerce.Ean13())
               .RuleFor(p => p.Name, f => f.Commerce.ProductName())
               .RuleFor(p => p.Price, f => f.Random.Number(100, 1000));

            var products = productFaker.Generate(30);
            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void SeedOrders(EShopDbContext context)
        {
            if (context.Orders.Any())
                return;
            var orderFaker = new Faker<Order>()
               .RuleFor(o => o.Id, f => f.Random.Guid())
               .RuleFor(o => o.Number, f => f.Random.Number(1, 100))
               .RuleFor(o => o.TotalPrice, f => f.Random.Float(1, 1000))
               .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
               .RuleFor(o => o.CreatedAt, f => f.Date.Past());

            var orders = orderFaker.Generate(10);
            
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
