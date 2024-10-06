

namespace Practica.Application.Interfaces
{
    internal interface IAuthService
    {
        Task <string> LoginAsync (string username, string password);
    }
}
