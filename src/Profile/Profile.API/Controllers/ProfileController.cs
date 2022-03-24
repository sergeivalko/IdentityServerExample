using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.Features.GetProfile;
using Profile.Application.Features.UpdateProfile;

namespace Profile.API.Controllers
{
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{profileId}")]
        public async Task<IActionResult> Get([FromRoute] Guid profileId)
        {
            var profile = await _mediator.Send(new GetProfileQuery(profileId));
            return Ok(profile);
        }

        [HttpPatch]
        [Route("{profileId}")]
        public async Task<IActionResult> Update([FromRoute] Guid profileId, [FromForm] UpdateProfileRequestDto body,
            List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formFile = files.FirstOrDefault();
            if (formFile == null)
            {
                await _mediator.Publish(new UpdateProfileCommand(profileId, body.FirstName, body.LastName, null));

                return Ok();
            }

            await using var stream = formFile.OpenReadStream();
            var bytes = new byte[formFile.Length];
            stream.Read(bytes);

            await _mediator.Publish(new UpdateProfileCommand(profileId, body.FirstName, body.LastName, bytes));
            return Ok();
        }
    }
}