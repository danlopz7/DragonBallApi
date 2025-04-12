using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Services.external.ExternalModels;
using DragonBallApi.Utilities;

namespace DragonBallApi.Services.external
{
    public class DragonBallApiClient : IDragonBallApiClient
    {
        private readonly HttpClient _httpClient;

        public DragonBallApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public Task<Result<List<ApiCharacter>>> GetSaiyanCharactersAsync()
        {
            var url = "https://dragonball-api.com/api/characters?limit=100&race=Saiyan";
            return GetFromApiAsync<List<ApiCharacter>>(url);
        }

        public Task<Result<List<ApiTransformation>>> GetTransformationsAsync()
        {
            var url = "https://dragonball-api.com/api/transformations";
            return GetFromApiAsync<List<ApiTransformation>>(url);
        }

        private async Task<Result<T>> GetFromApiAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Result<T>.Failure($"Error fetching data: {response.StatusCode}");
                }

                var data = await response.Content.ReadFromJsonAsync<T>();

                if (data == null)
                {
                    return Result<T>.Failure("No data returned from API.");
                }

                return Result<T>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Exception: {ex.Message}");
            }
        }
    }
}
