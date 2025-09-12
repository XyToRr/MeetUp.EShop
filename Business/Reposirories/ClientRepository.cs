//using Bogus;
//using DataAccess.Context;
//using MeetUp.EShop.Core.Interfaces;
//using MeetUp.EShop.Core.Models.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MeetUp.EShop.Business.Reposirories
//{
//    public class ClientRepository : IClientRepository
//    {
//        private readonly EShopDbContext _context;
//        public ClientRepository(EShopDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<Guid> AddClient(Client client)
//        {
//            client.Id = Guid.NewGuid();
//            _context.Clients.Add(client);
//            await _context.SaveChangesAsync();
//            return client.Id;
//        }

//        public Client? Get(Guid id)
//        {
//           return _context.Clients.FirstOrDefault(c => c.Id == id);
//        }

//        public IEnumerable<Client> GetClients()
//        {
//           return _context.Clients;
//        }

//        public async Task<bool> RemoveClient(Guid id)
//        {
//            var client = _context.Clients.FirstOrDefault(c => c.Id == id);

//            if (client == null)
//            {
//                return false;
//            }

//            _context.Clients.Remove(client);
//            return (await _context.SaveChangesAsync()) > 0;
//        }

//        public async Task<bool> UpdateClient(Client client)
//        {
//            var oldClient = _context.Clients.FirstOrDefault(c => c.Id == client.Id);
//            if (oldClient == null)
//            {
//                return false;
//            }
            
//            oldClient.FirstName = client.FirstName;
//            oldClient.LastName = client.LastName;
//            oldClient.Adress = client.Adress;
          
//            _context.Clients.Update(client);
//            return (await _context.SaveChangesAsync()) > 0;
//        }
//    }
//}
