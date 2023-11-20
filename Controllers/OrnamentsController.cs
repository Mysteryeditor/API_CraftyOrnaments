using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_CraftyOrnaments.Models;

namespace API_CraftyOrnaments.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrnamentsController : ControllerBase
    {
        private readonly CraftyOrnamentsContext _context;

        public OrnamentsController(CraftyOrnamentsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ornament>>> GetOrnaments()
        {
          if (_context.Ornaments == null)
          {
              return NotFound();
          }
            return await _context.Ornaments.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Ornament>> GetOrnament(short id)
        {
          if (_context.Ornaments == null)
          {
              return NotFound();
          }
            var ornament = await _context.Ornaments.FindAsync(id);

            if (ornament == null)
            {
                return NotFound();
            }

            return ornament;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrnament(short id, Ornament ornament)
        {
            if (id != ornament.OrnamentId)
            {
                return BadRequest();
            }

            _context.Entry(ornament).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrnamentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Ornament>> PostOrnament(Ornament ornament)
        {
          if (_context.Ornaments == null)
          {
              return Problem("Entity set 'CraftyOrnamentsContext.Ornaments'  is null.");
          }
            _context.Ornaments.Add(ornament);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrnament", new { id = ornament.OrnamentId }, ornament);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrnament(short id)
        {
            if (_context.Ornaments == null)
            {
                return NotFound();
            }
            var ornament = await _context.Ornaments.FindAsync(id);
            if (ornament == null)
            {
                return NotFound();
            }

            _context.Ornaments.Remove(ornament);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RingSize>>> GetSize()
        {
            if (_context.RingSizes== null)
            {
                return NotFound();
            }
            return await _context.RingSizes.ToListAsync();
        }


        private bool OrnamentExists(short id)
        {
            return (_context.Ornaments?.Any(e => e.OrnamentId == id)).GetValueOrDefault();
        }
    }
}
