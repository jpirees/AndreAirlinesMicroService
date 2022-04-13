
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Entities;
using Users.API.Services;
using Users.API.Validators;
using Utils.MongoDB;
using Xunit;

namespace Users.Test
{
    public class UnitTestUsers
    {
        public static (UserMongoService, UserValidator) InitializeDatabase()
        {
            var settings = new MongoDatabaseSettings("mongodb://localhost:27017", "AndreAirlinesMSTest", "Users");
            UserMongoService userMongoService = new(settings);
            UserValidator userValidator = new(userMongoService);

            return (userMongoService, userValidator);
        }

        [Fact]
        public async Task GetAll()
        {
            var (userMongoService, _) = InitializeDatabase();
            IEnumerable<User> users = await userMongoService.Get();
            Assert.Single(users);
        }

        [Fact]
        public async Task GetById()
        {
            var (userMongoService, _) = InitializeDatabase();
            IEnumerable<User> users = await userMongoService.Get();

            var user = users.FirstOrDefault();

            var userResponse = await userMongoService.Get(user.Id);
            Assert.Equal(user.Name, userResponse.Name);
        }

        [Fact]
        public async Task Post()
        {

            var newUser = new User("700.895.271-60", "Junior", DateTime.UtcNow, "(16) 99876-54321", "larissa@mail.com",
                new Address()
                {
                    ZipCode = "58079-740",
                    Street = "Rua Benedita Rodrigues de Vasconcelos",
                    Number = 317,
                    District = "Bairro das Lagostas",
                    City = "João Pessoa",
                    State = "PE",
                    Country = "Brasil",
                    Complement = "",
                    Continent = ""
                },
                "juniorsilva23",
                "12345",
                "Sales",
                new Role()
                {
                    Description = "manager",
                    Access = new List<Access>()
                    {
                        new Access()
                        {
                            Description = "airplanes"
                        },
                        new Access()
                        {
                            Description = "airports"
                        },
                        new Access()
                        {
                            Description = "flights"
                        },
                    }
                });


            var (_, userValidator) = InitializeDatabase();
            var (user, response) = await userValidator.ValidateToCreate(newUser);

            Assert.Equal(201, response.StatusCode);
            Assert.Equal(user.Name, newUser.Name);
        }

        [Fact]
        public async Task Update()
        {
            var (userMongoService, userValidator) = InitializeDatabase();

            var user = await userMongoService.GetByDocument("700.895.271-60");

            user.Name = "Junior Silva";

            var (userResponse, response) = await userValidator.ValidateToUpdate(user.Id, user);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(user.Name, userResponse.Name);
        }

        [Fact]
        public async Task Remove()
        {
            var (userMongoService, userValidator) = InitializeDatabase();

            var user = await userMongoService.GetByDocument("700.895.271-60");

            var (_, response) = await userValidator.ValidateToRemove(user.Id);

            Assert.Equal(200, response.StatusCode);
        }
    }
}
