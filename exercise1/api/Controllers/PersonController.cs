using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await _mediator.Send(new GetPeople()
                {

                });

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = result.Success,
                    Message = result.Message,
                    ResponseCode = result.ResponseCode,
                    MethodName = nameof(GetPeople)
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
                    MethodName = nameof(GetPeople)
                });

                return response;
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                });

                var response = this.GetResponse(result);

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = result.Success,
                    Message = result.Message,
                    ResponseCode = result.ResponseCode,
                    MethodName = nameof(GetPersonByName)
                });

                return response;
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
                    MethodName = nameof(GetPersonByName)
                });

                return response;
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePerson([FromBody] string name)
        {
            try
            {
                var result = await _mediator.Send(new CreatePerson()
                {
                    Name = name
                });

                var response = this.GetResponse(result);

                await _mediator.Send(new CreateAuditLog()
                {
                    Success = result.Success,
                    Message = result.Message,
                    ResponseCode = result.ResponseCode,
                    MethodName = nameof(CreatePerson)
                });

                return response;
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
                    MethodName = nameof(CreatePerson)
                });

                return response;
            }

        }
    }
}