namespace Application.RootUsers.Commands.CreateRootUser
{
    // public class CreateRootUserCommand : IRequest<RootUserViewModel>, IRegister
    // {
    //     public string FirstName { get; set; } = string.Empty;
    //     public string LastName { get; set; } = string.Empty;
    //     public DateTime Dob { get; set; }
    //     public string GithubLink { get; set; } = string.Empty;
    //     public string LinkedInLink { get; set; } = string.Empty;
    //
    //     public void Register(TypeAdapterConfig config)
    //     {
    //         config.ForType<CreateRootUserCommand, RootUser>().IgnoreNullValues(true);
    //     }
    // }
    //
    // public class CreateRootUserCommandHandler : IRequestHandler<CreateRootUserCommand, RootUserViewModel>
    // {
    //     private readonly IAppDbContext _context;
    //
    //     public CreateRootUserCommandHandler(IAppDbContext context)
    //     {
    //         _context = context;
    //     }
    //
    //     public async Task<RootUserViewModel> Handle(CreateRootUserCommand request, CancellationToken cancellationToken)
    //     {
    //         var entity = request.Adapt<RootUser>();
    //         _context.RootUsers.Add(entity);
    //
    //         await _context.SaveChangesAsync(cancellationToken);
    //
    //         return entity.Adapt<RootUserViewModel>();
    //     }
    // }
}