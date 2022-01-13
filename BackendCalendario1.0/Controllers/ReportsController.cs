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
    public class ReportsController : ControllerBase
    {
        private readonly CalendarioContext _context;

        public ReportsController(CalendarioContext context)
        {
            _context = context;
        }

        // GET: api/Reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisterDay>>> GetReports()
        {
            return await _context.RegisterDay.ToListAsync();
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reports>> GetReports(DateTime id)
        {
            var reports = await _context.Reports.FindAsync(id);

            if (reports == null)
            {
                return NotFound();
            }

            return reports;
        }

        // PUT: api/Reports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReports(DateTime id, Reports reports)
        {
            if (id != reports.date)
            {
                return BadRequest();
            }

            _context.Entry(reports).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportsExists(id))
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

        // POST: api/Reports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reports>> GetReportsByDay(Reports reports)
        {
            var response = await (from registerday in _context.RegisterDay
                                  where reports.date == registerday.date 
                                  select registerday).ToListAsync();
            //validacion de email y contraseña sean iguales en base de datos a la informacion proporcionada por el usuario
            if (reports.date == response[0].date)
            {
                return Ok(response.ToArray());
            }
           
            else
            {
                return BadRequest("No hay usuarios registrados en este dia");
            }
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReports(DateTime id)
        {
            var reports = await _context.Reports.FindAsync(id);
            if (reports == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(reports);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReportsExists(DateTime id)
        {
            return _context.Reports.Any(e => e.date == id);
        }
    }
}
