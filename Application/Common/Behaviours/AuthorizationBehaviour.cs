using System.Reflection;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public AuthorizationBehaviour(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

            if (authorizeAttributes.Any())
            {
                if (_currentUserService.UserId == null)
                {
                    throw new UnauthorizedException();
                }

                // Role-based authorization
                var authorizeAttributesWithRoles =
                    authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles)).ToList();

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(",")))
                    {
                        foreach (var role in roles)
                        {
                            var isInRole = await _identityService.IsInRoleAsync(_currentUserService.UserId.Value, role);
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    // Must be a member of at least one role in roles
                    if (!authorized)
                    {
                        throw new ForbiddenException();
                    }
                }

                // Policy-based authorization
                var authorizeAttributesWithPolicies =
                    authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy)).ToList();
                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {
                        var authorized =
                            await _identityService.AuthorizeAsync(_currentUserService.UserId.Value, policy);

                        if (!authorized)
                        {
                            throw new ForbiddenException();
                        }
                    }
                }
            }

            // User is authorized / authorization not required
            return await next();
        }
    }
}