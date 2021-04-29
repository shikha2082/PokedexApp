using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokedexApp.Model;
using Pokemondex.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokedexApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokemonController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET api/<PokemonsController>/5
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            PokemonBusinessLogic pokemonBL = new PokemonBusinessLogic(_httpClientFactory);
            Pokemon pokemon = pokemonBL.GetPokemon(name).Result;
            if (pokemon == null)
            {
                return NotFound("Record not found!");
            }

            return Ok(pokemon);
        }


        [Route("translated/{name}")]
        public IActionResult GetTranslatedPokemon(string name)
        {
            PokemonBusinessLogic pokemonBL = new PokemonBusinessLogic(_httpClientFactory);
            Pokemon pokemon = pokemonBL.GetTranslatedPokemon(name).Result;
            if (pokemon == null)
            {
                return NotFound("Record not found!");
            }

            return Ok(pokemon);
        }        
    }
}
