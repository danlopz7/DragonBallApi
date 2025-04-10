using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragonBallApi.Models;
using DragonBallApi.Data;

namespace DragonBallApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly IDragonBallService _dragonBallService;
        private readonly AppDbContext _context;

        public CharactersController(IDragonBallService dragonBallService, AppDbContext context)
        {
            _dragonBallService = dragonBallService;
            _context = context;
        }

        // POST /api/characters/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncCharacters()
        {
            var result = await _dragonBallService.SyncCharactersAsync();
            return Ok(new { message = result });
        }

         // GET /api/characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            var characters = await _context.Characters
                .Include(c => c.Transformations)
                .ToListAsync();

            return Ok(characters);
        }

        // GET /api/characters/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacterById(int id)
        {
            var character = await _context.Characters
                .Include(c => c.Transformations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }
    }
}