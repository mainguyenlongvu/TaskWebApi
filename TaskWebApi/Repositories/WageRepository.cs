using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories
{
    public interface IWageRepository
    {

        Task<WageEntity> GetWageById(string wageId);
        Task<List<WageEntity>> GetAllWage();
        Task<WageModel> CalculateWage(WageModel model);
    }
    public class WageRepository : IWageRepository
    {
        private readonly IMapper _mapper;
        private readonly TaskDbContext _context;
        private readonly UserManager<UserEntity> _userManager;
        public WageRepository(TaskDbContext context, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<WageModel> CalculateWage(WageModel model)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == model.UserId);
            var applicationApproved = await _context.Applications.Where(x => x.IsApproved == true)
            .ToListAsync();
            var applicationRejected = await _context.Applications.Where(x => x.IsRejected == true)
                 .ToListAsync();
            var applicationNotTest = await _context.Applications.Where(x => x.IsRejected == false && x.IsApproved == false)
                 .ToListAsync();
            int deduction = 200000;
            if (user == null)
            {
                throw new Exception("User is not exist");
            }
            var newWage = new WageEntity
            {
                Id = Guid.NewGuid().ToString(),
                DayOffApproved = applicationApproved.Count,
                DayOffRejected = applicationRejected.Count,
                Price = model.Price,
                UserId = model.UserId,
                Total = model.Price + model.Reward - applicationApproved.Count * deduction - applicationRejected.Count * deduction * 2 - applicationNotTest.Count * deduction * 2
            };

            _context.Wages.Add(newWage);
            _context.SaveChanges();
            var wage = _mapper.Map<WageModel>(newWage);
            return wage;
        }

        public async Task<List<WageEntity>> GetAllWage()
        {
            var wage = await _context.Wages
                 //.ProjectTo<WageModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();
            if(wage == null)
            {
                throw new Exception("Wage is null");
            }
            return wage;
        }

        public async Task<WageEntity> GetWageById(string wageId)
        {
            var existingWage = await _context.Wages.Where(f => f.Id == wageId)
           //.ProjectTo<WageModel>(_mapper.ConfigurationProvider)
           .SingleOrDefaultAsync();

            if (existingWage == null) throw new ArgumentException("Wage not found");

            return existingWage;
        }


    }
}
