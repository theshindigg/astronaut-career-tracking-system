using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AstronautDutyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                });

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = result.Success,
                    Message = result.Message,
                    ResponseCode = result.ResponseCode,
                    MethodName = nameof(GetAstronautDutiesByName)
                });

                return this.GetResponse(result); ;
            }
            catch (Exception ex)
            {
                var response = this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = false,
                    Message = ex.Message,
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    MethodName = nameof(GetAstronautDutiesByName)
                });

                return response;
            }            
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                var result = await _mediator.Send(request);

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = result.Success,
                    Message = result.Message,
                    ResponseCode = result.ResponseCode,
                    MethodName = nameof(CreateAstronautDuty)
                });

                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                var response = this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = false,
                    Message = ex.Message,
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    MethodName = nameof(CreateAstronautDuty)
                });

                return response;
            }         
        }
    }
}