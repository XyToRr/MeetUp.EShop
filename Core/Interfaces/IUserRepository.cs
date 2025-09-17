using MeetUp.EShop.Core.Models.User;

namespace MeetUp.EShop.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Register(RegisterUser user);
        User? Get(Guid guid);
        IEnumerable<User> GetUsers();
        Task<bool> Delete(Guid guid);
        Guid? GetByName(string name);
        Task<bool> Update(UpdateUser user);
        Task<bool> UpdateTokens(User user);
    }
}