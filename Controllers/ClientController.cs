using banking.DTOs;
using banking.Entities;
using banking.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IClientRepository clientRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetAllClients()
        {
            var clients = await clientRepository.GetClients();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClientById(int id)
        {
            var client = await clientRepository.GetClientById(id);
            if (client == null) return NotFound("Client not found.");
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientDto client)
        {
            await clientRepository.CreateClient(client);
            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, ClientDto client)
        {
            clientRepository.UpdateClient(id, client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            clientRepository.RemoveClient(id);
            return NoContent();
        }
    }
}
