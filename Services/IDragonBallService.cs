using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Utilities;

namespace DragonBallApi.Services
{
    public interface IDragonBallService
    {
        Task<Result<string>> SyncCharactersAsync();
        Task<Result<string>> ClearDatabaseAsync();
    }
}
