using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokedexApp.Model;
using Pokemondex.BusinessLogic;

namespace Pokemondex.UnitTest
{
    [TestClass]
    public class PokemonBusinessLogicUnitTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpClient> _mockHttpClient;

        [TestInitialize()]
        public void PokemonBusinessLogicInitialize()
        {
            _mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                        .ReturnsAsync(new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("[{'id':1,'value':'1'}]"),
                        });

            _mockHttpClientFactory.Setup(x => x.CreateClient()).Returns(_mockHttpClient.Object);

        }

        [TestMethod]
        public void CanGetPokemon()
        {
            //Arrange
            PokemonBusinessLogic pokemonBL = new PokemonBusinessLogic(_mockHttpClientFactory.Object);

            //Act
            var result = pokemonBL.GetPokemon("test");
            Pokemon pokemon = result.Result;

            //Assert
            Assert.IsNotNull(pokemon, "Invalid Result");
        }

        [TestMethod]
        public void CanGetTranslatedPokemon()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
