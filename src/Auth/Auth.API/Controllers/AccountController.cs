using System.Threading.Tasks;
using Auth.Application.Features.Commands.CreateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}