using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Services.external.ExternalModels;
using DragonBallApi.Utilities;

namespace DragonBallApi.Services.external
{
    public interface IDragonBallApiClient
    {
        Task<Result<List<ApiCharacter>>> GetSaiyanCharactersAsync();
        Task<Result<List<ApiTransformation>>> GetTransformationsAsync();
    }
}
