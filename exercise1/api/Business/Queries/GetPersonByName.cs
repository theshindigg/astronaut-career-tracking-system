using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class GetPersonByName : IRequest<GetPersonByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class GetPersonByNamePreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;
        public GetPersonByNamePreProcessor(StargateContext context)
        {
            _context = context;
        }
        public Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            if (person is null) throw new BadHttpRequestException($"No record exists for a Person with the name {request.Name}");

            return Task.CompletedTask;
        }
    }

    public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
    {
        private readonly StargateContext _context;
        public GetPersonByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
        {
            var result = new GetPersonByNameResult();

            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            var query = $"SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate FROM [Person] a LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id WHERE '{request.Name}' = a.Name";

            var previousDuty = _context.AstronautDuties.FirstOrDefault(z => z.PersonId == person!.Id);

            if (previousDuty == null)
            {
                query = $"SELECT a.Id as PersonId, a.Name FROM [Person] WHERE '{request.Name}' = a.Name";
            }

            var personAstronaut = await _context.Connection.QueryAsync<PersonAstronaut>(query);

            result.Person = personAstronaut.FirstOrDefault();

            return result;
        }
    }

    public class GetPersonByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
