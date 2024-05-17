using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TaskWebApi.Model;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Services;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WageController : ControllerBase
    {
        private readonly IWageService _wageService;
        //private readonly IMapper _mapper;

        public WageController(IWageService wageServide)
        {
            _wageService = wageServide;
            //_mapper = mapper;
        }

        [HttpGet]
        [Route("/api/[controller]/get-all-wage")]
        public async Task<ActionResult<List<WageEntity>>> GetAllWage()
        {

            try
            {
                var wage = await _wageService.GetAllWage();

                if (wage == null) return NoContent();

                return Ok(wage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/get-wage-by-Id")]
        public async Task<ActionResult<WageEntity>> GetWageById(string wageId)
        {

            try
            {
                var wage = await _wageService.GetWageById(wageId);
                return Ok(wage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("/api/[controller]/calculate-wage")]
        public async Task<ActionResult<WageModel>> CalculateWage(WageModel model)
        {
            try
            {
                var result = await _wageService.CalculateWage(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
