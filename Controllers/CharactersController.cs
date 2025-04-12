using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Data;
using DragonBallApi.DTOs;
using DragonBallApi.Models;
using DragonBallApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(new { message = result.Data });
        }

        // DELETE /api/characters/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearDatabase()
        {
            var result = await _dragonBallService.ClearDatabaseAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(new { message = result.Data });
        }

        // GET /api/characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetCharacters()
        {
            var characters = await _context
                .Characters.Include(c => c.Transformations)
                .ToListAsync();

            var dtoList = characters.Select(c => new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                Ki = c.Ki,
                Race = c.Race,
                Gender = c.Gender,
                Description = c.Description,
                Affiliation = c.Affiliation,
                Transformations = c
                    .Transformations.Select(t => new TransformationDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Ki = t.Ki,
                    })
                    .ToList(),
            });

            return Ok(dtoList);
        }

        // GET /api/characters/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacterById(int id)
        {
            var character = await _context
                .Characters.Include(c => c.Transformations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }
    }
}
