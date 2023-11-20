using API_CraftyOrnaments.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API_CraftyOrnaments.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MetalChoicesController : ControllerBase
    {
        private readonly CraftyOrnamentsContext _context;

        public MetalChoicesController(CraftyOrnamentsContext context)
        {
            _context = context;
        }

        [HttpGet]
        //[ApiKey]
        public ActionResult<IEnumerable<MetalChoice>> GetMetalChoices()
        {
            if (_context.MetalChoices == null)
            {
                return NotFound();
            }
            return _context.MetalChoices.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetalChoice>> GetSingleMetal(short id)
        {
            if (_context.MetalChoices == null)
            {
                return NotFound();
            }
            var metalChoice = await _context.MetalChoices.FindAsync(id);

            if (metalChoice == null)
            {
                return NotFound();
            }

            return metalChoice;
        }

        [HttpPut]
        public async Task<IActionResult> PutMetalChoice(short id, MetalChoice metalChoice)
        {
            MetalChoice? existingData = _context.MetalChoices.Find(id);
            if (metalChoice.MetalImage.Length <= 0)
            {

                if (existingData != null)
                {
                    existingData.PurityGrade = metalChoice.PurityGrade;
                    existingData.MarketPrice = metalChoice.MarketPrice;
                    existingData.CreatedDate = DateTime.Now;
                    existingData.ModifiedDate = DateTime.Now;
                    _context.Entry(existingData).State = EntityState.Modified;
                }
            }





            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetalChoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost]
        public ResponseType<string> PostMetalChoice(MetalChoice metalChoice)
        {
            ResponseType<string> response = new ResponseType<string>
            {
                StatusCode = 200,
                Message = "Success",
            };

            if (metalChoice is not null)
            {
                metalChoice.CreatedDate= DateTime.Now;
                metalChoice.ModifiedDate= DateTime.Now;
               
                try
                {
                    _context.MetalChoices.Add(metalChoice);
                    _context.SaveChanges();
                    return response;
                }

                catch (Exception ex)
                {
                    response = new ResponseType<string>
                    {
                        StatusCode = 500,
                        Message = ex.Message,
                    };
                    return response;

                }
            }
            else
            {
                response = new ResponseType<string>
                {
                    StatusCode = 500,
                    Message = "Server Error Try Later",
                };
                return response;
            }
           
          

          
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetalChoice(short id)
        {
            if (_context.MetalChoices == null)
            {
                return NotFound();
            }
            var metalChoice = await _context.MetalChoices.FindAsync(id);
            if (metalChoice == null)
            {
                return NotFound();
            }

            _context.MetalChoices.Remove(metalChoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MetalChoiceExists(short id)
        {
            return (_context.MetalChoices?.Any(e => e.MetalId == id)).GetValueOrDefault();
        }
    }
}
