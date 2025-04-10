using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonBallApi.Services
{
    public interface IDragonBallService
    {
        Task<string> SyncCharactersAsync();
    }
}