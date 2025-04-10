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
            // Verifica si ya hay datos
            bool hasCharacters = await _context.Characters.AnyAsync();
            bool hasTransformations = await _context.Transformations.AnyAsync();
            if (hasCharacters || hasTransformations)
            {
                return "La base de datos ya tiene datos. Limpia las tablas antes de sincronizar.";
            }

            //var response = await _httpClient.GetAsync("https://dragonball-api.com/api/characters");
            var response = await _httpClient.GetAsync("https://dragonball-api.com/api/characters?limit=58&race=Saiyan");

            if (!response.IsSuccessStatusCode)
            {
                return "No se pudo obtener datos desde la API externa.";
            }

            var apiCharacters = await response.Content.ReadFromJsonAsync<List<ApiCharacter>>();
            //var saiyanCharacters = apiCharacters.Where(c => c.Race == "Saiyan").ToList();
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
                    Transformations = new List<Transformation>(),
                };

                if (apiChar.Affiliation == "Z Fighter" && apiChar.Transformations != null)
                {
                    foreach (var trans in apiChar.Transformations)
                    {
                        character.Transformations.Add(
                            new Transformation { Name = trans.Name, Ki = trans.Ki }
                        );
                    }
                }
                charactersToSave.Add(character);
            }

            _context.Characters.AddRange(charactersToSave);
            await _context.SaveChangesAsync();
            return "Sincronización completada con éxito.";
        }

        // Modelos auxiliares para mapear el JSON del API
        private class ApiCharacter
        {
            public string Name { get; set; }
            public string Ki { get; set; }
            public string Race { get; set; }
            public string Gender { get; set; }
            public string Description { get; set; }
            public string Affiliation { get; set; }
            public List<ApiTransformation> Transformations { get; set; }
        }

        private class ApiTransformation
        {
            public string Name { get; set; }
            public string Ki { get; set; }
        }
    }
}
