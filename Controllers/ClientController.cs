// using necessary namespaces
using System.Security.Claims;
using System.Text.Json;
using banking.DTOs;
using banking.Extensions;
using banking.Helpers;
using banking.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// Define the namespace for the controller
namespace banking.Controllers
{
    // Authorize only admin users to access these endpoints
    [Authorize(Roles = "Admin")]
    // Define the route prefix for this controller
    [Route("api/[controller]")]
    // Indicate that this is an API controller
    [ApiController]
    public class ClientController(IClientRepository clientRepository) : ControllerBase
    {
        private readonly IClientRepository _clientRepository = clientRepository; // Dependency injection of client repository

        // GET: api/client
        // Get a list of clients, filtered by parameters and authenticated user
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetAllClients([FromQuery] ClientParams clientParams)
        {
            var clients = await _clientRepository.GetClients(clientParams, User.GetUserId());
            Response.AddPaginationHeader(clients); // Add custom header with pagination information
            return Ok(clients);
        }

        // GET: api/client/{personalId}
        // Get a single client by personal ID and authenticated user
        [HttpGet("{personalId}")]
        public async Task<ActionResult<ClientDto?>> GetClientByPersonalId(string personalId)
        {
            var client = await _clientRepository.GetClientByPersonalId(personalId, User.GetUserId());
            if (client == null) return NotFound("Client not found.");
            return Ok(client);
        }

        // GET: api/client/Suggestions
        // Get search suggestions based on the authenticated user's history
        [HttpGet("Suggestions")]
        public async Task<ActionResult<List<SearchHistoryDto>>> GetSuggestions()
        {
            var Suggestions = await _clientRepository.GetSearchSuggestions(User.GetUserId());
            return Ok(Suggestions);
        }

        // POST: api/client
        // Create a new client with the provided data and returns a 201 Created status code
        [HttpPost]
        public async Task<IActionResult> AddClient(CreateClientDto client)
        {
            await _clientRepository.CreateClient(client);
            return Created(); // Return a 201 response with Location header set to the new resource's URL
        }

        // PUT: api/client/{id}
        // Update an existing client by ID, or return a 404 Not Found status code if not found
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, UpdateClientDto client)
        {
            if (await _clientRepository.UpdateClient(id, client) == false) return NotFound("Client not found.");
            return NoContent(); // Return a 204 response to indicate successful update
        }

        // DELETE: api/client/{id}
        // Delete an existing client by ID, or return a 404 Not Found status code if not found
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (await _clientRepository.RemoveClient(id) == false) return NotFound("Client not found.");
            return NoContent(); // Return a 204 response to indicate successful deletion
        }
    }
}
