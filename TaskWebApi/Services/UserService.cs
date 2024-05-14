using TaskWebApi.Repositories;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Model;
using TaskWebApi.Enum;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TaskWebApi.Services
{
    public interface IUserService
    {
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByIdAsync(string id);
        Task<UserEntity> CreateUserAsync(UserModel userModel);
        Task<UserEntity> Register(RegisterModel registerModel);
        Task<UserEntity> UpdateUserAsync(string id, UserModel userModel);
        Task<ResponseLoginModel> LoginAsync(RequestLoginModel requestLoginModel);
        Task DeleteUserAsync(string id);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;


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
        public async Task<ResponseLoginModel> LoginAsync(RequestLoginModel requestLoginModel)
        {
            var userRepository = _unitOfWork.UserRepository;
            var login = await userRepository.CheckLoginAsync(requestLoginModel);
            return login;
        }

        public async Task<UserEntity> Register(RegisterModel registerModel)
        {
            var user = _unitOfWork.UserRepository;
            if (await user.GetByEmailAsync(registerModel.Email) != null)
                throw new Exception($"Email: {registerModel.Email} is exit!");


            var newUser = _mapper.Map<UserEntity>(registerModel);

            var result = await user.CreateAsync(newUser);
            return newUser;
        }


        public async Task<UserEntity> GetUserByIdAsync(string id)
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

        public async Task<UserEntity> UpdateUserAsync(string  id, UserModel userDto)
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

        public async Task DeleteUserAsync(string id)
        {
            var userRepository = _unitOfWork.UserRepository;
            var deletedUser = await userRepository.GetByIdAsync(id);
            deletedUser.IsActive = false;
            await userRepository.UpdateAsync(deletedUser);

            await _unitOfWork.SaveChangesAsync();
        }
    }

}

