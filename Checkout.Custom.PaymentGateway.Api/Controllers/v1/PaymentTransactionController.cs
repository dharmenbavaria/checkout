using Checkout.Custom.Common.Model;
using Checkout.Custom.PaymentGateway.Services.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Custom.PaymentGateway.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("execute")]
        public async Task<ActionResult<PaymentTransactionResponseVm>> ExecutePayment([FromBody] ExecutePayment command)
        {
            var transactionId = await _mediator.Send(command);
            return Ok(await _mediator.Send(new GetTransactionById { Id = transactionId }));
        }

        [HttpGet("{id}", Name = nameof(GetTransactionById))]
        public async Task<ActionResult<PaymentTransactionResponseVm>> GetTransactionById(Guid id)
        {
            return Ok( await _mediator.Send(new GetTransactionById { Id = id }));
        }

        [HttpGet("merchants/{merchantId}", Name = nameof(GetTransactionByMerchantId))]
        public async Task<ActionResult<List<PaymentTransactionResponseVm>>> GetTransactionByMerchantId(Guid merchantId)
        {
            return Ok(await _mediator.Send(new GetTransactionByMerchantId { MerchantId = merchantId }));
        }

    }
}
