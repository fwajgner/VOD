namespace Tests.APITests.ControllersTests
{
    using Entities;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using System.Net.Http;

    public class GenreControllerTest : BaseHttpTest
    {
        public class ReadAllAsync : GenreControllerTest
        {
            private IEnumerable<Genre> Genres { get; set; } = new Genre[]
            {
                new Genre { Name = "Drama" },
                new Genre { Name = "Animation" },
                new Genre { Name = "Comedy" }
            };

            protected override void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton(Genres);
            }


            [Fact]
            public async Task Should_return_the_default_genres()
            {
                //Arrange
                int expectedNumberOfGenres = Genres.Count();

                //Act
                HttpResponseMessage result = await Client.GetAsync("v1/genre");

                //Assert
                result.EnsureSuccessStatusCode();
                Genre[] genres = await result.Content.ReadAsAsync<Genre[]>();
                Assert.NotNull(genres);
                Assert.Equal(expectedNumberOfGenres, genres.Length);
                Assert.Collection(genres,
                        genre => Assert.Equal(Genres.ElementAt(0).Name, genres[0].Name),
                        genre => Assert.Equal(Genres.ElementAt(1).Name, genres[1].Name),
                        genre => Assert.Equal(Genres.ElementAt(2).Name, genres[2].Name)
                    );
            }
        }      
    }
}
