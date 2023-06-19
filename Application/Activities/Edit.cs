// Edit Class

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        // Edit Command
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        // Edit Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Handler Logic
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null)
                    throw new Exception("Could not find activity");

                _mapper.Map(request.Activity, activity);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }

}