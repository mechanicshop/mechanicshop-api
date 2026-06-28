using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.Identity.Queries.RefreshTokens;

public record RefreshTokenQuery(string RefreshToken, string ExpiredAccessToken) : IRequest<Result<TokenResponse>>;
