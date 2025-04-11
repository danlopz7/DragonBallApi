using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Services.external.ExternalModels;

namespace DragonBallApi.Services.external
{
    public interface IDragonBallApiClient
    {
        Task<List<ApiCharacter>> GetSaiyanCharactersAsync();
        Task<List<ApiTransformation>> GetTransformationsAsync();
    }
}