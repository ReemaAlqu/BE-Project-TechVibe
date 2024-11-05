using src.Utils;
using static src.DTO.UserDTO;

namespace src.Services.UserService
{
    public interface IUserService
    {
        Task<UserReadDto> CreateOneAsync(UserCreateDto createDto);
        Task<List<UserReadDto>> GetAllAsync();
        Task<UserReadDto> GetByIdAsync(Guid id);
        Task<bool> DeleteOneAsync(Guid id);
        Task<bool> UpdateOneAsync(Guid id, UserUpdateDto updateDto);
        Task<string> SignInAsync(UserSignInDto signInDto);
    }
}
