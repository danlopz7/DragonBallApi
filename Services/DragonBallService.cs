using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Data;
using DragonBallApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DragonBallApi.Services
{
    public class DragonBallService : IDragonBallService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public DragonBallService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> SyncCharactersAsync()
        {
            // Checking for existing data
            bool hasCharacters = await _context.Characters.AnyAsync();
            bool hasTransformations = await _context.Transformations.AnyAsync();
            if (hasCharacters || hasTransformations)
            {
                return "The database already has data. Clean up the tables before synchronizing.";
            }

            // Obtain Saiyan characters from API
            var characterResponse = await _httpClient.GetAsync(
                "https://dragonball-api.com/api/characters?race=Saiyan"
            );

            if (!characterResponse.IsSuccessStatusCode)
            {
                return "Could not get data from external API.";
            }

            var apiCharacters = await characterResponse.Content.ReadFromJsonAsync<
                List<ApiCharacter>
            >();
            if (apiCharacters == null || !apiCharacters.Any())
            {
                return "No Saiyan characters found.";
            }

            var charactersToSave = new List<Character>();

            foreach (var apiChar in apiCharacters)
            {
                var character = new Character
                {
                    Name = apiChar.Name,
                    Ki = apiChar.Ki,
                    Race = apiChar.Race,
                    Gender = apiChar.Gender,
                    Description = apiChar.Description,
                    Affiliation = apiChar.Affiliation,
                    Transformations = new List<Transformation>()
                };

                charactersToSave.Add(character);
            }

            _context.Characters.AddRange(charactersToSave);
            await _context.SaveChangesAsync();

            // Obtain transformations
            var transResponse = await _httpClient.GetAsync(
                            "https://dragonball-api.com/api/transformations"
                        );
            if (!transResponse.IsSuccessStatusCode)
            {
                return "Could not get transformations from the API";
            }

            var apiTransformations = await transResponse.Content.ReadFromJsonAsync<
                List<ApiTransformation>
            >();
            if (apiTransformations == null)
            {
                return "No transformations were found.";
            }

            // Associate transformations from characters whose affiliation is "Z Fighter"
            var zFighters = await _context.Characters
            .Where(c => c.Affiliation != null && c.Affiliation.ToLower() == "z fighter")
            .ToListAsync();

            // Filter Z Fighter characters
            var zFighterCharacters = apiCharacters
                .Where(c =>
                    !string.IsNullOrEmpty(c.Affiliation)
                    && c.Affiliation.Equals("Z Fighter", StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            var transformationsToSave = new List<Transformation>();

            foreach (var trans in apiTransformations)
            {
                // Find a character whose name is contained in the transformation name
                var matchingCharacter = zFighters.FirstOrDefault(c =>
                    !string.IsNullOrEmpty(trans.Name) &&
                    trans.Name.Contains(c.Name, StringComparison.OrdinalIgnoreCase));

                if (matchingCharacter != null)
                {
                    transformationsToSave.Add(new Transformation
                    {
                        Name = trans.Name,
                        Ki = trans.Ki,
                        CharacterId = matchingCharacter.Id
                    });
                }
            }

            _context.Transformations.AddRange(transformationsToSave);
            await _context.SaveChangesAsync();

            return "Synchronization completed successfully.";
        }

        public async Task<string> ClearDatabaseAsync()
        {
            _context.Transformations.RemoveRange(_context.Transformations);
            _context.Characters.RemoveRange(_context.Characters);
            await _context.SaveChangesAsync();

            return "Database successfully cleaned.";
        }

        // Helper models to map API JSON
        private class ApiCharacter
        {
            public string Name { get; set; }
            public string Ki { get; set; }
            public string Race { get; set; }
            public string Gender { get; set; }
            public string Description { get; set; }
            public string Affiliation { get; set; }
        }

        private class ApiTransformation
        {
            public string Name { get; set; }
            public string Ki { get; set; }
        }
    }
}
