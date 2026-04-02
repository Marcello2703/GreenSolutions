using GreenSolutions.Models;
using GreenSolutions.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //[HttpGet]
        //public async GetUser()
        //{

        //}

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            _appDbContext.UsersDB.Add(user);
            await _appDbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("getAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _appDbContext.UsersDB.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserById(int id)
        {
            var user = await _appDbContext.UsersDB.FindAsync(id);

            if (user == null)
            {
                return NotFound("Usuario não encontrado.");
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await _appDbContext.UsersDB.FindAsync(id);
            if(user == null)
            {
                return NotFound("Usuario não encontrado.");
            }

            _appDbContext.Entry(user).CurrentValues.SetValues(updatedUser);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(201, updatedUser);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _appDbContext.UsersDB.FindAsync(id);
            if (user == null)
            {
                return NotFound("Usuario não encontrado.");
            }

            _appDbContext.UsersDB.Remove(user);
            await _appDbContext.SaveChangesAsync();

            return Ok("Usuario deletado.");
        }
    }
}
