using System.Net;
using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Models;
using MeetUp.EShop.Core.Models.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeetUp.EShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Route("getClients")]
        public IResult GetClients()
        {
            var clients = _clientService.GetClients();
            if (clients == null || !clients.Any())
            {
                throw new ControllerException("Not found clients", HttpStatusCode.NotFound);
            }
            Log.Information("Retrieved {Count} clients successfully", clients.Count());
            return Results.Ok(clients);
        }

        [HttpGet]
        [Route("getClient")]
        public IResult GetClient(Guid id)
        {
            var client = _clientService.Get(id);
            if (client == null)
            {
                throw new ControllerException($"Not found client with id: {id}", HttpStatusCode.NotFound);
            }
            Log.Information("Retrieved client with ID {ClientId} successfully", id);
            return Results.Ok(client);
        }

        [HttpPost]
        [Route("addClient")]
        public async Task<IResult> AddClient(Client client)
        {
            var result = await _clientService.AddClient(client);
            if (result == Guid.Empty)
            {
                throw new ControllerException("Bad addClient request", HttpStatusCode.BadRequest);
            }
            Log.Information("Added client with ID {ClientId} successfully", result);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("updateClient")]
        public async Task<IResult> UpdateClient(Client client)
        {
            var result = await _clientService.UpdateClient(client);
            if (!result)
            {
                throw new ControllerException("Bad updateClient request", HttpStatusCode.BadRequest);
            }
            Log.Information("Updated client with ID {ClientId} successfully", client.Id);
            return Results.Ok();
        }

        [HttpDelete]
        [Route("deleteClient")]
        public async Task<IResult> DeleteClient(Guid id)
        {
            var result = await _clientService.RemoveClient(id);
            if (!result)
            {
                throw new ControllerException("Bad deleteClient request", HttpStatusCode.BadRequest);
            }
            Log.Information("Deleted client with ID {ClientId} successfully", id);
            return Results.Ok();
        }
    }
}
