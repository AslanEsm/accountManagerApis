using Entities.User;
using System.Threading.Tasks;
using ViewModels.Token;

namespace Services.Interfaces
{
    public interface IJwtService
    {
        Task<UserToken> GenerateAsync(User user);
    }
}