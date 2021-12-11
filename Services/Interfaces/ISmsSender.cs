using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISmsSender
    {
        Task<string> SendAuthSmsAsync(string code, string phoneNumber);
    }
}