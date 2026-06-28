using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Identity.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.Identity.Queries.GetUserInfo;

public sealed record GetUserByIdQuery(string? UserId) : IRequest<Result<AppUserDto>>;