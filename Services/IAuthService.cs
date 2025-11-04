using CrimeManagement.DTOs;

namespace CrimeManagment.Services
{
    public interface IAuthService
    {
        Task<AuthDtos.LoginResponseDto> LoginAsync(AuthDtos.LoginDtos dto);
    }
}