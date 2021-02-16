using System;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MaxshoesBack.JwtAuth
{
    public interface IJwtAuthManager
    {
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }

        JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);

        JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);

        void RemoveExpiredRefreshTokens(DateTime now);

        void RemoveRefreshTokenByUserName(string userName);

        (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);

        string GenerateConfirmEmailToken(string username, Claim[] claims, DateTime now);

        string GenerateTemporaryPasswordString();

        string GeneratePasswordResetToken(Claim[] claims, DateTime now);

        string ConfirmEmailToken(string UserName, string Token, DateTime Now);

        string ConfirmPasswordResetToken(string Email, string Token);
    }
}