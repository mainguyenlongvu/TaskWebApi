using TaskWebApi.Repositories;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Model;
using TaskWebApi.Enum;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TaskWebApi.CoreHelper.Helper;

namespace TaskWebApi.Services
{
    public interface IUserService
    {
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByIdAsync(string id);
        //Task<UserEntity> CreateUserAsync(UserModel userModel);
        Task<IdentityResult> Register(RegisterModel registerModel);
        Task<UserEntity> UpdateUserAsync(string id, UserModel userModel);
        Task<string> LoginAsync(RequestLoginModel requestLoginModel);
        Task<IActionResult> ConfirmEmail(string email, string otp);

        Task DeleteUserAsync(string id);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SendMail _sendMail;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper, SendMail sendMail)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sendMail = sendMail;
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            var userRepository = _unitOfWork.UserRepository;
            var users = await userRepository.GetAllAsync();
            return users;
        }

        public async Task<string> LoginAsync(RequestLoginModel requestLoginModel)
        {
            var userRepository = _unitOfWork.UserRepository;
            var login = await userRepository.CheckLoginAsync(requestLoginModel);
            return login;
        }

        public async Task<IdentityResult> Register(RegisterModel registerModel)
        {
            var user = _unitOfWork.UserRepository;
            if (await user.GetByEmailAsync(registerModel.Email) != null)
                throw new Exception($"Email: {registerModel.Email} already exists!");

            var result = await user.CreateAsync(registerModel);
            return result;
        }

        public async Task<IActionResult> ConfirmEmail(string email, string otp)
        {
            var user = await _unitOfWork.UserRepository.ConfirmEmail(email, otp);

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(user);
        }


        public async Task<UserEntity> GetUserByIdAsync(string id)
        {
            var userRepository = _unitOfWork.UserRepository;
            var user = await userRepository.GetByIdAsync(id);
            return user;
        }

        //public async Task<UserEntity> CreateUserAsync(RegisterModel registerModel)
        //{
        //    var userRepository = _unitOfWork.UserRepository;
        //    var user = _mapper.Map<UserEntity>(userDto);
        //    await userRepository.CreateAsync(UserModel);
        //    await _unitOfWork.SaveChangesAsync();
        //    return user;
        //}

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

