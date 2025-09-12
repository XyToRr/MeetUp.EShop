using MeetUp.EShop.Core.Models.Client;

namespace MeetUp.EShop.Core.Interfaces;
public interface IClientRepository
{
    Task<Guid> AddClient(Client client);
    IEnumerable<Client> GetClients();
    Client? Get(Guid id);
    Task<bool> RemoveClient(Guid id);
    Task<bool> UpdateClient(Client client);
}