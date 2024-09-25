using System.Security.Claims;
using System.Text.Json;
using banking.DTOs;
using banking.Extensions;
using banking.Helpers;
using banking.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace banking.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IClientRepository clientRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetAllClients([FromQuery]ClientParams clientParams)
        {
            var clients = await clientRepository.GetClients(clientParams, User.GetUserId());
            Response.AddPaginationHeader(clients);
            return Ok(clients);
        }

        [HttpGet("{personalId}")]
        public async Task<ActionResult<ClientDto>?> GetClientByPersonalId(string personalId)
        {
            var client = await clientRepository.GetClientByPersonalId(personalId, User.GetUserId());
            if (client == null) return NotFound("Client not found.");
            return Ok(client);
        }

        [HttpGet("Suggestions")]
        public async Task<ActionResult<List<SearchHistoryDto>>> GetSuggestions()
        {
            var Suggestions = await clientRepository.GetSearchSuggestions(User.GetUserId());
            return Ok(Suggestions);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(CreateClientDto client)
        {
            await clientRepository.CreateClient(client);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, UpdateClientDto client)
        {
            if (await clientRepository.UpdateClient(id, client) == false) return NotFound("Client not found.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (await clientRepository.RemoveClient(id) == false) return NotFound("Client not found.");

            return NoContent();
        }
    }
}
