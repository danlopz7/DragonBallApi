using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Data;
using DragonBallApi.Models;
using DragonBallApi.Services.external;
using DragonBallApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DragonBallApi.Services
{
    public class DragonBallService : IDragonBallService
    {
        private readonly AppDbContext _context;
        private readonly IDragonBallApiClient _apiClient;

        public DragonBallService(AppDbContext context, IDragonBallApiClient apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }

        public async Task<Result<string>> SyncCharactersAsync()
        {
            // Checking for existing data
            bool hasCharacters = await _context.Characters.AnyAsync();
            bool hasTransformations = await _context.Transformations.AnyAsync();

            if (hasCharacters || hasTransformations)
            {
                return Result<string>.Failure(
                    "The database already has data. Clean up the tables before synchronizing."
                );
            }

            // Obtain Saiyan characters from API
            var characterResult = await _apiClient.GetSaiyanCharactersAsync();
            if (
                !characterResult.IsSuccess
                || characterResult.Data == null
                || !characterResult.Data.Any()
            )
            {
                return Result<string>.Failure(
                    characterResult.ErrorMessage ?? "No Saiyan characters found."
                );
            }

            var charactersToSave = characterResult
                .Data.Select(apiChar => new Character
                {
                    Name = apiChar.Name,
                    Ki = apiChar.Ki,
                    Race = apiChar.Race,
                    Gender = apiChar.Gender,
                    Description = apiChar.Description,
                    Affiliation = apiChar.Affiliation,
                    Transformations = new List<Transformation>(),
                })
                .ToList();

            _context.Characters.AddRange(charactersToSave);
            await _context.SaveChangesAsync();

            // Obtain transformations
            var transformationsResult = await _apiClient.GetTransformationsAsync();
            if (
                !transformationsResult.IsSuccess
                || transformationsResult.Data == null
                || !transformationsResult.Data.Any()
            )
            {
                return Result<string>.Failure(
                    transformationsResult.ErrorMessage ?? "No transformations were found."
                );
            }

            // Associate transformations from characters whose affiliation is "Z Fighter"
            var zFighters = await _context
                .Characters.Where(c =>
                    c.Affiliation != null && c.Affiliation.ToLower() == "z fighter"
                )
                .ToListAsync();

            var transformationsToSave = new List<Transformation>();

            foreach (var trans in transformationsResult.Data)
            {
                // Find a character whose name is contained in the transformation name
                var matchingCharacter = zFighters.FirstOrDefault(c =>
                    !string.IsNullOrEmpty(trans.Name)
                    && trans.Name.Contains(c.Name, StringComparison.OrdinalIgnoreCase)
                );

                if (matchingCharacter != null)
                {
                    transformationsToSave.Add(
                        new Transformation
                        {
                            Name = trans.Name,
                            Ki = trans.Ki,
                            CharacterId = matchingCharacter.Id,
                        }
                    );
                }
            }

            _context.Transformations.AddRange(transformationsToSave);
            await _context.SaveChangesAsync();

            return Result<string>.Success("Synchronization completed successfully.");
        }

        public async Task<Result<string>> ClearDatabaseAsync()
        {
            try
            {
                _context.Transformations.RemoveRange(_context.Transformations);
                _context.Characters.RemoveRange(_context.Characters);
                await _context.SaveChangesAsync();

                return Result<string>.Success("Database successfully cleaned.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure("Failed to clear database: " + ex.Message);
            }
        }
    }
}
