using AutoMapper;
using System;
using TaskWebApi.Model;
using TaskWebApi.Repositories;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Services
{
    public interface IWageService
    {
        Task<WageEntity> GetWageById(string wageId);
        Task<List<WageEntity>> GetAllWage();
        Task<WageModel> CalculateWage(WageModel model);
    }
    public class WageService : IWageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<WageModel> CalculateWage(WageModel model)
        {
            var wageRepository = _unitOfWork.WageRepository;
            var result = await wageRepository.CalculateWage(model);
            return result;
        }

        public async Task<List<WageEntity>> GetAllWage()
        {
            var wageRepository = _unitOfWork.WageRepository;
            var result = await wageRepository.GetAllWage();
            return result;
        }

        public async Task<WageEntity> GetWageById(string wageId)
        {
            var wageRepository = _unitOfWork.WageRepository;
            var result = await wageRepository.GetWageById(wageId);
            return result;
        }
    }
}
