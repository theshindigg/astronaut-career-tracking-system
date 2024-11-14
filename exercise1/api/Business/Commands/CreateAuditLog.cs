using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands
{
    public class CreateAuditLog: IRequest<CreateAuditLogResult>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int ResponseCode { get; set; }

        public string MethodName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
    public class CreateAuditLogHandler : IRequestHandler<CreateAuditLog, CreateAuditLogResult>
    {
        private readonly StargateContext _context;

        public CreateAuditLogHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<CreateAuditLogResult> Handle(CreateAuditLog request, CancellationToken cancellationToken)
        {
            var AuditLog = new AuditLog()
            {
                Success = request.Success,
                Message = request.Message,
                ResponseCode = request.ResponseCode,
                MethodName = request.MethodName,
                CreatedAt = DateTime.Now
                
            };

            await _context.AuditLogs.AddAsync(AuditLog);

            await _context.SaveChangesAsync();

            return new CreateAuditLogResult()
            {
                Success = request.Success,
                Message = request.Message,
                ResponseCode = request.ResponseCode,
                MethodName = request.MethodName,
                CreatedAt = AuditLog.CreatedAt
            };
        }
    }


    public class CreateAuditLogResult: BaseResponse
    {
        public string MethodName { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
