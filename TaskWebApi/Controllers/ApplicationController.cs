using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.SqlServer.Server;
using System;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Services;
using static System.Net.Mime.MediaTypeNames;
using ApplicationModel = TaskWebApi.Model.ApplicationModel;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationService applicationService, IMapper mapper)
        {
            _applicationService = applicationService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/api/[controller]/get-all-application")]
        public async Task<ActionResult<List<ApplicationModel>>> GetAllAppliction()
        {

            try
            {
                var application = await _applicationService.GetAllApplication();

                if (application == null) return NoContent();

                return Ok(application);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/get-application-by-Id")]
        public async Task<ActionResult<ApplicationModel>> GetApplictionById(string applicationId)
        {

            try
            {
                var application = await _applicationService.GetApplicationById(applicationId);

                var response = _mapper.Map<ApplicationModel>(application);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("/api/[controller]/sent-appliction")]
        public async Task<ActionResult<ApplicationModel>> SentApplication([FromForm] ApplicationModel applicationModel, ICollection<IFormFile> files)
        {
            try
            {
                var result = await _applicationService.SentApplication(applicationModel, files);
                //var application = _mapper.Map<ApplicationModel>(result);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

            [HttpPut]
        [Route("/api/[controller]/approve-appliction")]
        public async Task<ActionResult<ApplicationModel>> ApproveAppliction(string applicationId)
        {
            try
            {
                var application = await _applicationService.ApproveApplication(applicationId);

                return Ok(application);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("/api/[controller]/reject-appliction")]
        public async Task<ActionResult<ApplicationModel>> RejectAppliction(string applicationId)
        {
            try
            {
                var application = await _applicationService.RejectApplication(applicationId);

                return Ok(application);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        


    }
}
