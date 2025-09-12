using MeetUp.EShop.Core.Models.User;

namespace MeetUp.EShop.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Register(User user);
        User? Get(Guid guid);
        IEnumerable<User> GetUsers();
        Guid? GetByName(string name);
        Task<bool> Update(User user);
        Task<bool> UpdateTokens(User user);
    }
}