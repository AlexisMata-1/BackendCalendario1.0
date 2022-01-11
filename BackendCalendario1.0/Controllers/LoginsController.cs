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
    public class LoginsController : ControllerBase
    {
        private readonly CalendarioContext _context;

        public LoginsController(CalendarioContext context)
        {
            _context = context;
        }

        // GET: api/Logins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetCredentials()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Logins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Login>> GetLogin(string id)
        {
            var login = await _context.Login.FindAsync(id);

            if (login == null)
            {
                return NotFound();
            }

            return login;
        }

        // PUT: api/Logins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogin(string id, Login login)
        {
            if (id != login.email)
            {
                return BadRequest();
            }

            _context.Entry(login).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id))
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

        // POST: api/Logins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> GetUserByEmail(Users userCredentials)
        {
            var response = await (from user in _context.Users
                                  where userCredentials.email == user.email && user.is_active == true
                                  select user).ToListAsync();
            userCredentials.pass = Encrypt.GetSHA256(userCredentials.pass);
            if (userCredentials.pass == response[0].pass)
            {
                return Ok(response[0]);
            }
            else
            {
                return BadRequest("Contraseña o usuario incorrecto");
            }
        }

        // DELETE: api/Logins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogin(string id)
        {
            var login = await _context.Login.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }

            _context.Login.Remove(login);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoginExists(string id)
        {
            return _context.Login.Any(e => e.email == id);
        }
    }
}
