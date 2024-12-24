using MediatR;
using Microsoft.AspNetCore.Mvc;
using Assignment.Domain.Entities;
using Assignment.Application.Trial.Queries;
using Assignment.Application.Trial.Requests;

namespace Assignment.Web.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TrialController(IGetTrialByTrialIdQuery getTrialBtTrialIdQuery, IGetTrailsByFilter getTrailsByFilter) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadTrial(IFormFile file, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UploadFileRequest(file), cancellationToken);
            if (result.IsSuccess)
                return Ok();
            if (result.Exception != null)
                return StatusCode(500, new
                {
                    Message = result.Exception.Message
                });
            return BadRequest(result.Message);
        }

        [HttpGet("{trialId}")]
        public async Task<IActionResult> GetByTrialId(string trialId)
        {
            var trial = await getTrialBtTrialIdQuery.ExecuteAsync(trialId);
            return Ok(trial);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrials(
            string? trialId = null,
            string? title = null,
            TrialStatus? status = null)
        {
            var items = await getTrailsByFilter.ExecuteAsync(trialId, title, status);
            return Ok(items);

        }
    }
}
