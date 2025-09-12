using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Client;

namespace MeetUp.EShop.Business.Services;

public class ClientService (IClientRepository clientRepository)
{
    public async Task<Guid> AddClient(Client client)
    {
        if (client.FirstName == string.Empty)
            return Guid.Empty;
        if (client.LastName == string.Empty)
            return Guid.Empty;
        if (client.Adress == string.Empty)
            return Guid.Empty;
        return await clientRepository.AddClient(client);
    }
    public IEnumerable<Client> GetClients() => clientRepository.GetClients();
    public Client? Get(Guid id) => clientRepository.Get(id);
    public async Task<bool> RemoveClient(Guid id) => await clientRepository.RemoveClient(id);
    public async Task<bool> UpdateClient(Client client) => await clientRepository.UpdateClient(client);
}