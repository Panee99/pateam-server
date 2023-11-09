using Application.Common.Interfaces;
using Application.RootUsers.Models;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.RootUsers.Commands.UpdateRootUser
{
    public class UpdateRootUserCommand : IRequest<RootUserViewModel>, IRegister
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string GithubLink { get; set; } = string.Empty;
        public string LinkedInLink { get; set; } = string.Empty;

        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateRootUserCommand, RootUser>().IgnoreNullValues(true);
        }
    }

    public class UpdateRootUserCommandHandler : IRequestHandler<UpdateRootUserCommand, RootUserViewModel>
    {
        private readonly IAppDbContext _context;

        public UpdateRootUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<RootUserViewModel> Handle(UpdateRootUserCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Adapt<RootUser>();
            
            _context.RootUsers.Update(entity);
            
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Adapt<RootUserViewModel>();
        }
    }
}