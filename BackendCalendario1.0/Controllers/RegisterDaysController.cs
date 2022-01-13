using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCalendario1._0.Models;

namespace BackendCalendario1._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterDaysController : ControllerBase
    {
        private readonly CalendarioContext _context;

        public RegisterDaysController(CalendarioContext context)
        {
            _context = context;
        }

        // GET: api/RegisterDays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisterDay>>> GetRegisterDay()
        {
            return await _context.RegisterDay.ToListAsync();
        }

        // GET: api/RegisterDays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisterDay>> GetRegisterDay(int id)
        {
            var registerDay = await _context.RegisterDay.FindAsync(id);

            if (registerDay == null)
            {
                return NotFound();
            }

            return registerDay;
        }

        // PUT: api/RegisterDays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegisterDay(int id, RegisterDay registerDay)
        {
            if (id != registerDay.id_register_day)
            {
                return BadRequest();
            }

            _context.Entry(registerDay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisterDayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Datos cambiados exitosamente");
        }

        // POST: api/RegisterDays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RegisterDay>> PostRegisterDay(RegisterDay registerDay)
        {
           if (registerDay == null) { 
                return BadRequest("No se envió nungun usuarios para registrar");
            }
            else
            {
                var response = await (from register in _context.RegisterDay
                                      where register.date == registerDay.date && register.id_user == registerDay.id_user
                                      select register).FirstOrDefaultAsync();
                if(response== null)
                {
                    _context.RegisterDay.Add(registerDay);
                    await _context.SaveChangesAsync();
                    return Ok((from register in _context.RegisterDay
                               where register.date == registerDay.date && register.id_user == registerDay.id_user
                               select register).FirstOrDefault());
                }
                response.confirmed_assist = true;
                await _context.SaveChangesAsync();
                return Ok((from register in _context.RegisterDay
                           where register.date == registerDay.date && register.id_user == registerDay.id_user
                           select register).FirstOrDefault());
            }
        }

        // DELETE: api/RegisterDays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisterDay(int id)
        {
            var registerDay = await _context.RegisterDay.FindAsync(id);
            if (registerDay == null)
            {
                return NotFound();
            }

            _context.RegisterDay.Remove(registerDay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegisterDayExists(int id)
        {
            return _context.RegisterDay.Any(e => e.id_register_day == id);
        }
    }
}
