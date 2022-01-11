using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCalendario1._0.Models;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BackendCalendario1._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CalendarioContext _context;

        public UsersController(CalendarioContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id_user)
        {
            var users = await _context.Users.FindAsync(id_user);
            if (users == null)
            {
                return NotFound();
            }
            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int Id_user, Users users)
        {
            Users userinfo = await (from user in _context.Users
                                    where user.id_user == Id_user && user.is_active == true
                                    select user).FirstOrDefaultAsync();
            if (userinfo == null)
            {
                return BadRequest("El usuario no existe");
            }
            else
            {
                try
                {
                    PropertyInfo[] userprops = typeof(Users).GetProperties();
                    foreach (var props in userprops)
                    {
                        var userinfoact = props.GetValue(userinfo).ToString();
                        var useract = props.GetValue(users) == null ? "" : props.GetValue(users).ToString();
                        if (userinfoact != useract && props.Name != "pass")
                        {
                            props.SetValue(userinfo, props.GetValue(users));
                        }
                        if (props.Name == "pass" && useract != "")
                        {
                            props.SetValue(userinfo, Encrypt.GetSHA256(props.GetValue(users).ToString()));
                        }
                    }
                    await _context.SaveChangesAsync();
                    return Ok(userinfo);
                }
                catch
                {
                    return BadRequest("Hubo un error al intentar cambiar tu informacion");
                }
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUser(Users users)
        {
            
            //validacion para solo mayores de 17 y menores de 75 años
            DateTime actualdate = DateTime.Now;
            if ((actualdate.Year - users.dob.Year) < 18)
            {
                return BadRequest("La fecha ingresada es menor a la limite de registro");
            }
            else if ((actualdate.Year - users.dob.Year) > 70)
            {
                return BadRequest("La fecha ingresada es mayor a la limite de registro");
            }
            //Validacion de correos para solo arkus y arkus-soluitions
            Regex rege = new Regex("@arkus.com", RegexOptions.Compiled);
            Regex rege1 = new Regex("@arkus[a-z-]+.com", RegexOptions.Compiled);
            MatchCollection matches = rege.Matches(users.email);
            MatchCollection matches1 = rege1.Matches(users.email);
            if (matches.Count == 0 && matches1.Count == 0)
            {
                return BadRequest("¡El correo que ingreso es incorrecto!");
            }
            else
            {
                var query = _context.Users.Where(x => x.email == users.email).FirstOrDefault(); // Check del correo en base de datos
                if (query != null) // Si se encuentra un correo se envia un badrequest
                {
                    return BadRequest("!El correo que ingreso ya es existente en el sistema!");
                }
                var pass = users.pass.Length;
                if (pass< 9)
                {
                    return BadRequest("La contraseña debe contener minimo 9 caracteres");
                }
                else //Si no pasa a registrar el correo
                {
                    users.pass = Encrypt.GetSHA256(users.pass); //Hash del password
                    _context.Users.Add(users); // Agregado de la info al contexto
                    await _context.SaveChangesAsync(); //Guardado del contexto en base de datos
                    return CreatedAtAction("GetUsers", new { Id_user = users.id_user }, users);
                }

            }
        }

        [HttpDelete("{Id_user}")]
        public async Task<ActionResult<Users>> DeleteUser(int Id_user)
        {
            var user = await _context.Users.FindAsync(Id_user);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [HttpDelete]
        public async Task<ActionResult<Users>> DeleteUsers([FromBody] List<int> users)
        {
            try
            {
                List<Users> user = await _context.Users.Where(x => (users.Contains(x.id_user))).ToListAsync();

                user.ForEach(x =>
                {
                    _context.Users.Remove(x);
                });
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id_user == id);
        }
    }
}
