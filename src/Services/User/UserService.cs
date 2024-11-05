using AutoMapper;
using src.Entity;
using src.Repository;
using src.Utils;
using static src.DTO.UserDTO;
using static src.Entity.User;

namespace src.Services.UserService
{
    public class UserService : IUserService
    {
        protected readonly UserRepository _userRepo;
        protected readonly IMapper _mapper;
        protected readonly IConfiguration _configuration;

        public UserService(UserRepository userRepo, IMapper mapper, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserReadDto> CreateOneAsync(UserCreateDto createDto)
        {
            var existingUser = await _userRepo.FindByEmailAsync(createDto.EmailAddress);
            if (existingUser != null)
            {
                throw CustomException.BadRequest(
                    $"User with email '{createDto.EmailAddress}' already exists."
                );
            }
            PasswordUtils.HashPassword(
                createDto.Password,
                out string hashedPassword,
                out byte[] salt
            );
            var user = _mapper.Map<UserCreateDto, User>(createDto);
            user.Password = hashedPassword;
            user.Salt = salt;
            user.UserRole = Role.Customer;
            var userCreated = await _userRepo.CreateOneAsync(user);
            return _mapper.Map<User, UserReadDto>(userCreated);
        }

        //get all original method
        public async Task<List<UserReadDto>> GetAllAsync()
        {
            var userList = await _userRepo.GetAllAsync();
            return _mapper.Map<List<User>, List<UserReadDto>>(userList);
        }

        
        public async Task<UserReadDto> GetByIdAsync(Guid id)
        {
            var foundUser = await _userRepo.GetByIdAsync(id);
            return _mapper.Map<User, UserReadDto>(foundUser);
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var foundUser = await _userRepo.GetByIdAsync(id);
            if (foundUser != null)
            {
                bool isDeleted = await _userRepo.DeleteOneAsync(foundUser);
                if (isDeleted)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdateOneAsync(Guid id, UserUpdateDto updateDto)
        {
            var foundUser = await _userRepo.GetByIdAsync(id);

            if (foundUser == null)
            {
                throw CustomException.BadRequest($"User with ID '{id}' not found.");
            }
            _mapper.Map(updateDto, foundUser);
            return await _userRepo.UpdateOneAsync(foundUser);
        }

        public async Task<string> SignInAsync(UserSignInDto signInDto)
        {
            var foundUser = await _userRepo.FindByEmailAsync(signInDto.EmailAddress);
            if (foundUser == null)
            {
                throw CustomException.BadRequest(
                    $"User with email '{signInDto.EmailAddress}' not found."
                );
            }
            var isMatched = PasswordUtils.VerifyPassword(
                signInDto.Password,
                foundUser.Password,
                foundUser.Salt
            );
            if (isMatched)
            {
                var tokenUtil = new TokenUtils(_configuration);
                return tokenUtil.GenerateToken(foundUser);
            }
            throw CustomException.UnAuthorized("Password does not match.");
        }
    }
}
