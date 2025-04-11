using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Services.external.ExternalModels;

namespace DragonBallApi.Services.external
{
    public class DragonBallApiClient : IDragonBallApiClient
    {
        private readonly HttpClient _httpClient;

        public DragonBallApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<ApiCharacter>> GetSaiyanCharactersAsync()
        {
            var response = await _httpClient.GetAsync("https://dragonball-api.com/api/characters?limit=100&race=Saiyan");
            if (!response.IsSuccessStatusCode)
            {
                //return "Could not get data from external API.";
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ApiCharacter>>();
        }

        public async Task<List<ApiTransformation>> GetTransformationsAsync()
        {
            var response = await _httpClient.GetAsync("https://dragonball-api.com/api/transformations");
            if (!response.IsSuccessStatusCode)
            {
                //return "Could not get data from external API.";
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ApiTransformation>>();
        }
    }
}