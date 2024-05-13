using TaskWebApi.Repositories;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Model;
using TaskWebApi.Enum;
using AutoMapper;

namespace TaskWebApi.Services
{
    public interface IUserService
    {
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByIdAsync(int id);
        Task<UserEntity> CreateUserAsync(UserModel userModel);
        Task<UserEntity> UpdateUserAsync(int id, UserModel userModel);
        Task DeleteUserAsync(int id);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            var userRepository = _unitOfWork.UserRepository;
            var users = await userRepository.GetAllAsync();
            return users;
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            var userRepository = _unitOfWork.UserRepository;
            var user = await userRepository.GetByIdAsync(id);
            return user;
        }

        public async Task<UserEntity> CreateUserAsync(UserModel userDto)
        {
            var userRepository = _unitOfWork.UserRepository;
            var user = _mapper.Map<UserEntity>(userDto);
            await userRepository.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity> UpdateUserAsync(int id, UserModel userDto)
        {
            var userRepository = _unitOfWork.UserRepository;
            var existingUser = await userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new Exception("User does not exist");
            }
            _mapper.Map(userDto, existingUser);
            await userRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return existingUser;
        }

        public async Task DeleteUserAsync(int id)
        {
            var userRepository = _unitOfWork.UserRepository;
            var deletedUser = await userRepository.GetByIdAsync(id);
            deletedUser.IsActive = false;
            await userRepository.UpdateAsync(deletedUser);

            await _unitOfWork.SaveChangesAsync();
        }
    }

}

