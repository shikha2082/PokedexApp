using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokedexApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokemondex.BusinessLogic
{
    public class PokemonBusinessLogic
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokemonBusinessLogic(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get Pokemon by name
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetPokemon(string pokemonName)
        {
            Pokemon pokemon = new Pokemon()
            {
                PokemonName = pokemonName
            };

            string requesturl = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonName;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requesturl);

                var client = _httpClientFactory.CreateClient();

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic dynamicObj = (JObject)JsonConvert.DeserializeObject(json);

                    if (dynamicObj != null)
                    {
                        //Get Description
                        var description = dynamicObj.flavor_text_entries;
                        if (description != null)
                        {
                            pokemon.Description = description[0].flavor_text;
                        }

                        //Get Habitat
                        pokemon.Habitat = dynamicObj?.habitat?.name;
                        pokemon.IsLegendary = dynamicObj?.is_legendary;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return pokemon;
        }

        /// <summary>
        /// Get translated description for a pokemon
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetTranslatedPokemon(string pokemonName)
        {
            Pokemon pokemon;
            string requesturl = null;
            
            try
            {
                pokemon = GetPokemon(pokemonName)?.Result;

                if (pokemon == null)
                {
                    return null;
                }

                if (pokemon.IsLegendary || pokemon.Habitat == "cave")
                {
                    requesturl = "https://api.funtranslations.com/translate/yoda.json";
                }
                else
                {
                    requesturl = "https://api.funtranslations.com/translate/shakespeare.json";
                }

                var request = new HttpRequestMessage(HttpMethod.Post, requesturl);

                var client = _httpClientFactory.CreateClient();

                var content = new List<KeyValuePair<string, string>>();
                content.Add(new KeyValuePair<string, string>("text", pokemon.Description));

                request.Content = new FormUrlEncodedContent(content);                 

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic dynamicObj = (JObject)JsonConvert.DeserializeObject(json);

                    if (dynamicObj != null)
                    {
                        //Get Description
                        var description = dynamicObj.contents;
                        if (description != null)
                        {
                            pokemon.Description = description.translated;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return pokemon;
        }
    }
}
