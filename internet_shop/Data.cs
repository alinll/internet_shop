using internet_shop.Models;
using Microsoft.EntityFrameworkCore;

namespace internet_shop
{
    public class Data
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShoppingContext(serviceProvider.GetRequiredService<DbContextOptions<ShoppingContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Customers.Any())
                {
                    return;
                }

                context.Roles.AddRange(
                    new Role
                    {
                        Name = "admin"
                    },
                    new Role
                    {
                        Name = "buyer"
                    }
                    );

                context.SaveChanges();

                context.Customers.AddRange(
                    new Customer
                    {
                        FirstName = "Ostap",
                        LastName = "Bender",
                        Address = "Rio de Zhmerinka",
                        Email = "admin@gmail.com",
                        Password = "123456",
                        RoleId = 1
                    },
                    new Customer
                    {
                        FirstName = "Shura",
                        LastName = "Balaganov",
                        Address = "Odessa",
                        Email = "buyer1@gmail.com",
                        Password = "buyer1",
                        RoleId = 2
                    },
                    new Customer
                    {
                        FirstName = "Zosya",
                        LastName = "Sinitsina",
                        Address = "Chernomorsk",
                        Email = "buyer2@gmail.com",
                        Password = "buyer2",
                        RoleId = 2
                    },
                    new Customer
                    {
                        FirstName = "Semiuel M.",
                        LastName = "Panikovskiy",
                        Address = "Chernomorsk",
                        Email = "buyer3@gmail.com",
                        Password = "buyer3",
                        RoleId = 2
                    },
                    new Customer
                    {
                        FirstName = "Adam",
                        LastName = "Kozlevych",
                        Address = "Chernomorsk",
                        Email = "buyer4@gmail.com",
                        Password = "buyer4",
                        RoleId = 2
                    },
                    new Customer
                    {
                        FirstName = "Madam",
                        LastName = "Gritsatsuieva",
                        Address = "Ensk",
                        Email = "buyer5@gmail.com",
                        Password = "buyer5",
                        RoleId = 2
                    }
                    );

                context.SaveChanges();

                context.Products.AddRange(
                        new Product
                        {
                            Name = "Butter",
                            Price = 30.0F,
                            Count = 96
                        },
                        new Product
                        {
                            Name = "Banana",
                            Price = 20.50F,
                            Count = 70
                        },
                        new Product
                        {
                            Name = "Cola",
                            Price = 9.30F,
                            Count = 50
                        },
                        new Product
                        {
                            Name = "Honey",
                            Price = 13F,
                            Count = 42
                        },
                        new Product
                        {
                            Name = "Soup",
                            Price = 13F,
                            Count = 91
                        },
                        new Product
                        {
                            Name = "Rice",
                            Price = 4F,
                            Count = 8
                        },
                        new Product
                        {
                            Name = "Coffee",
                            Price = 57F,
                            Count = 67
                        },
                        new Product
                        {
                            Name = "Potatoes",
                            Price = 17F,
                            Count = 6
                        }
                    );

                context.SaveChanges();

                context.Orders.AddRange(
                    new Order
                    {
                        CustomerId = 1,
                        OrderDate = DateTime.Now,
                        ProductId = 1,
                        Quantity = 2
                    },
                    new Order
                    {
                        CustomerId = 1,
                        OrderDate = DateTime.Now,
                        ProductId = 2,
                        Quantity = 1
                    },
                    new Order
                    {
                        CustomerId = 2,
                        OrderDate = new DateTime(2023, 08, 14),
                        ProductId = 1,
                        Quantity = 1
                    },
                    new Order
                    {
                        CustomerId = 3,
                        OrderDate = new DateTime(2022, 10, 15),
                        ProductId = 4,
                        Quantity = 7
                    },
                    new Order
                    {
                        CustomerId = 4,
                        OrderDate = new DateTime(2023, 01, 25),
                        ProductId = 3,
                        Quantity = 5
                    },
                    new Order
                    {
                        CustomerId = 5,
                        OrderDate = new DateTime(2023, 05, 14),
                        ProductId = 2,
                        Quantity = 2
                    },
                    new Order
                    {
                        CustomerId = 6,
                        OrderDate = new DateTime(2023, 07, 19),
                        ProductId = 6,
                        Quantity = 1
                    },
                    new Order
                    {
                        CustomerId = 6,
                        OrderDate = new DateTime(2023, 06, 09),
                        ProductId = 7,
                        Quantity = 5
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
