using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskWebApi.Model;
using TaskWebApi.Services;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;

        public AttachmentController(IAttachmentService attachmentService, IMapper mapper)
        {
            _attachmentService = attachmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/api/[controller]/get-all-application")]
        public async Task<ActionResult<List<AttachmentModel>>> GetAllAttachment()
        {

            try
            {
                var attachment = await _attachmentService.GetAllAttachment();

                if (attachment == null) return NoContent();

                return Ok(attachment);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
