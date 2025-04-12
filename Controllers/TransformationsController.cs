using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonBallApi.Data;
using DragonBallApi.DTOs;
using DragonBallApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DragonBallApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransformationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransformationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/transformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransformationDto>>> GetTransformations()
        {
            var transformations = await _context.Transformations.ToListAsync();

            var dtoList = transformations.Select(t => new TransformationDto
            {
                Id = t.Id,
                Name = t.Name,
                Ki = t.Ki,
            });

            return Ok(dtoList);
        }
    }
}
