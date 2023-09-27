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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountProfilesController : ControllerBase
    {
        private readonly CraftyOrnamentsContext _context;

        public AccountProfilesController(CraftyOrnamentsContext context)
        {
            _context = context;
        }

        // GET: api/AccountProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountProfile>>> GetAccountProfiles()
        {
          if (_context.AccountProfiles == null)
          {
              return NotFound();
          }
            return await _context.AccountProfiles.ToListAsync();
        }

        // GET: api/AccountProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountProfile>> GetAccountProfile(int id)
        {
          if (_context.AccountProfiles == null)
          {
              return NotFound();
          }
            var accountProfile = await _context.AccountProfiles.FindAsync(id);

            if (accountProfile == null)
            {
                return NotFound();
            }

            return accountProfile;
        }

        // PUT: api/AccountProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountProfile(int id, AccountProfile accountProfile)
        {
            if (id != accountProfile.UserId)
            {
                return BadRequest();
            }

            _context.Entry(accountProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountProfileExists(id))
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

        // POST: api/AccountProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountProfile>> PostAccountProfile(AccountProfile accountProfile)
        {
          if (_context.AccountProfiles == null)
          {
              return Problem("Entity set 'CraftyOrnamentsContext.AccountProfiles'  is null.");
          }
            _context.AccountProfiles.Add(accountProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountProfile", new { id = accountProfile.UserId }, accountProfile);
        }

        // DELETE: api/AccountProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountProfile(int id)
        {
            if (_context.AccountProfiles == null)
            {
                return NotFound();
            }
            var accountProfile = await _context.AccountProfiles.FindAsync(id);
            if (accountProfile == null)
            {
                return NotFound();
            }

            _context.AccountProfiles.Remove(accountProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountProfileExists(int id)
        {
            return (_context.AccountProfiles?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
