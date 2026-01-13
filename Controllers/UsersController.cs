using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportMafiaSpiel;
using SportMafiaSpiel.Models;
using System.Linq;
using System;

namespace SportMafiaSpiel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SportMafiaSpielContext _context;

        public UsersController(SportMafiaSpielContext context)
        {
            _context = context;
        }

        // GET api/users/{id} — Benutzerprofil abrufen
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);

            if (user == null)
                return NotFound("Benutzer nicht gefunden");

            // Konvertieren von User zu UserDto, damit PasswordHash nicht gesendet wird
            var userDto = new UserDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };

            return Ok(userDto);
        }

        // POST api/users — neuen Benutzer erstellen
        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            // Wenn CreatedAt nicht angegeben, setze aktuelle Zeit
            if (newUser.CreatedAt == default)
                newUser.CreatedAt = DateTime.Now;

            // Benutzer in die Datenbank hinzufügen
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // UserDto zurückgeben, ohne Passwort
            var userDto = new UserDto
            {
                UserID = newUser.UserID,
                Username = newUser.Username,
                Email = newUser.Email,
                CreatedAt = newUser.CreatedAt,
                IsActive = newUser.IsActive
            };

            return Ok(userDto);
        }
    }
}
