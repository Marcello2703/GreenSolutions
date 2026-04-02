using GreenSolutions.Models;
using GreenSolutions.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ClientController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(Client client)
        {
            _appDbContext.ClientsDB.Add(client);
            await _appDbContext.SaveChangesAsync();

            return Ok(client);
        }

        [HttpGet("getAllClients")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _appDbContext.ClientsDB.ToListAsync();
            if (!clients.Any()) { return NotFound("Não existem clientes."); }

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientById(int id)
        {
            var client = await _appDbContext.ClientsDB.FindAsync(id);

            if (client == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client updatedClient)
        {
            var client = await _appDbContext.ClientsDB.FindAsync(id);
            if (client == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            _appDbContext.Entry(client).CurrentValues.SetValues(updatedClient);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(201, updatedClient);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _appDbContext.ClientsDB.FindAsync(id);
            if (client == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            _appDbContext.ClientsDB.Remove(client);
            await _appDbContext.SaveChangesAsync();

            return Ok("Cliente deletado.");
        }
    }
}
